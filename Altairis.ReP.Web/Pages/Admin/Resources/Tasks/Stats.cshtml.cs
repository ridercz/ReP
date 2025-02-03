namespace Altairis.ReP.Web.Pages.Admin.Resources.Tasks;

public class StatsModel(RepDbContext dc, IOptions<AppSettings> options) : PageModel {

    public string ResourceName { get; set; } = string.Empty;

    public ICollection<string> TaskNames { get; set; } = [];

    public record StatInfo(string TaskName, string UserDisplayName, int Count);

    public ICollection<StatInfo[]> Records { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int resourceId) {
        // Get resource name
        var resource = await dc.Resources.FindAsync(resourceId);
        if (resource == null) return this.NotFound();
        this.ResourceName = resource.Name;

        this.TaskNames = await dc.MaintenanceTasks.Where(x => x.ResourceId == resourceId).Select(x => x.Name).ToListAsync();
        for (var monthOffset = 0; monthOffset <= options.Value.Maintenance.ShowStatsForMonths; monthOffset++) {
            // Get first and last day of current month
            var firstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-monthOffset);
            var lastDay = firstDay.AddMonths(1).AddSeconds(-1);

            // Get maintenance records for current month, grouped by task and user
            var monthlyRecords = await dc.MaintenanceRecords
                .Where(x => x.ResourceId == resourceId && x.Date >= firstDay && x.Date <= lastDay)
                .GroupBy(x => new { x.MaintenanceTask.Name, x.User.DisplayName })
                .Select(x => new StatInfo(x.Key.Name, x.Key.DisplayName, x.Count()))
                .ToArrayAsync();
            this.Records.Add(monthlyRecords);
        }

        return this.Page();
    }

}
