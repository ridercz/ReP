namespace Altairis.ReP.Web.Pages.My;

public class ResourceMaintenanceModel(RepDbContext dc, TimeProvider timeProvider) : PageModel {

    // Output model

    public int ResourceId { get; set; }

    public string ResourceName { get; set; } = string.Empty;

    public record MaintenanceTaskInfo(int Id, string Name, string Interval);

    public IEnumerable<MaintenanceTaskInfo> MaintenanceTasks { get; set; } = [];

    public record MaintenanceRecordInfo(int Id, string Name, DateTime Date, string User, bool CanBeDeleted);

    public IEnumerable<MaintenanceRecordInfo> MaintenanceRecords { get; set; } = [];

    // Handlers

    public async Task<IActionResult> OnGetAsync(int resourceId) {
        var isPrivilegedUser = this.User.IsInRole(ApplicationRole.Administrator) || this.User.IsInRole(ApplicationRole.Master);
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
            .Select(x => new MaintenanceRecordInfo(x.Id, x.MaintenanceTask.Name, x.Date, x.User.DisplayName, isPrivilegedUser || x.User.UserName!.Equals(this.User.Identity!.Name)))
            .ToListAsync();
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int resourceId, int recordId) {
        var isPrivilegedUser = this.User.IsInRole(ApplicationRole.Administrator) || this.User.IsInRole(ApplicationRole.Master);
        var record = await dc.MaintenanceRecords.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == recordId && x.ResourceId == resourceId);
        if (record != null && (isPrivilegedUser || record.User.UserName!.Equals(this.User.Identity!.Name))) {
            dc.MaintenanceRecords.Remove(record);
            await dc.SaveChangesAsync();
        }
        return this.RedirectToPage(new { resourceId });
    }

}
