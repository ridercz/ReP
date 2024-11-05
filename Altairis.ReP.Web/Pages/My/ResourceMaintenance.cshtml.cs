namespace Altairis.ReP.Web.Pages.My;

public class ResourceMaintenanceModel(RepDbContext dc, TimeProvider timeProvider) : PageModel {

    // Output model

    public int ResourceId { get; set; }

    public string ResourceName { get; set; } = string.Empty;

    public record MaintenanceTaskInfo(int Id, string Name, string Interval);

    public IEnumerable<MaintenanceTaskInfo> MaintenanceTasks { get; set; } = [];

    public record MaintenanceRecordInfo(string Name, DateTime Date, string User);

    public IEnumerable<MaintenanceRecordInfo> MaintenanceRecords { get; set; } = [];

    // Handlers

    public async Task<IActionResult> OnGetAsync(int resourceId) {
        var resource = await dc.Resources.FindAsync(resourceId);
        if (resource == null) return this.NotFound();
        this.ResourceId = resourceId;
        this.ResourceName = resource.Name;
        this.MaintenanceTasks = await dc.MaintenanceTasks.Where(x => x.ResourceId == resourceId).Select(x => new MaintenanceTaskInfo(x.Id, x.Name, x.Interval)).ToListAsync();
        this.MaintenanceRecords = await dc.MaintenanceRecords
            .Where(x => x.ResourceId == resourceId && x.Date >= timeProvider.GetLocalNow().Date.AddDays(-30))
            .OrderByDescending(x => x.Date)
            .Include(x => x.MaintenanceTask)
            .Include(x => x.User)
            .Select(x => new MaintenanceRecordInfo(x.MaintenanceTask.Name, x.Date, x.User.DisplayName))
            .ToListAsync();
        return this.Page();
    }


}
