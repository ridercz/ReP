namespace Altairis.ReP.Web.Pages.Admin.Resources.Tasks;

public class IndexModel(RepDbContext dc) : PageModel
{
    // Output model

    public int ResourceId { get; set; }

    public string ResourceName { get; set; } = string.Empty;

    public record TaskInfo(int Id, string Name, string Interval);

    public IEnumerable<TaskInfo> Items { get; set; } = [];

    // Handlers

    public async Task<IActionResult> OnGetAsync(int resourceId) {
        // Load resource name
        var resource = await dc.Resources.FindAsync(resourceId);
        if (resource == null) return this.NotFound();
        this.ResourceId = resourceId;
        this.ResourceName = resource.Name;

        // Load tasks
        this.Items = await dc.MaintenanceTasks
            .Where(x => x.ResourceId == resourceId)
            .OrderBy(x => x.Name)
            .Select(x => new TaskInfo(x.Id, x.Name, x.Interval))
            .ToListAsync();

        return this.Page();

    }
}
