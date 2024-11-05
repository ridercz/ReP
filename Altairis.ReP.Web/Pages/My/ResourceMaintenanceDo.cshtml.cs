using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My;

public class ResourceMaintenanceDoModel(RepDbContext dc, TimeProvider timeProvider, UserManager<ApplicationUser> userManager) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

    }

    // Output model

    public int ResourceId { get; set; }

    public string ResourceName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Interval { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;


    // Handlers

    public async Task<IActionResult> OnGetAsync(int resourceId, int taskId) {
        if (!await this.Init(resourceId, taskId)) return this.NotFound();
        this.Input.Time = timeProvider.GetLocalNow().DateTime;
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int resourceId, int taskId) {
        if (!await this.Init(resourceId, taskId)) return this.NotFound();
        if (!this.ModelState.IsValid) return this.Page();

        // Get ID of current user
        var user = await userManager.GetUserAsync(this.User) ?? throw new ImpossibleException();

        // Create maintenance record
        var newRecord = new MaintenanceRecord {
            Date = this.Input.Time,
            MaintenanceTaskId = taskId,
            ResourceId = resourceId,
            UserId = user.Id
        };
        dc.MaintenanceRecords.Add(newRecord);
        await dc.SaveChangesAsync();

        return this.RedirectToPage("ResourceMaintenance", new { resourceId });
    }

    // Helpers

    public async Task<bool> Init(int resourceId, int taskId) {
        var task = await dc.MaintenanceTasks.Include(x => x.Resource).SingleOrDefaultAsync(x => x.Id == taskId && x.ResourceId == resourceId);
        if (task == null) return false;

        this.ResourceId = task.ResourceId;
        this.ResourceName = task.Resource.Name;
        this.Name = task.Name;
        this.Interval = task.Interval;
        this.Description = task.Description ?? string.Empty;

        return true;
    }

}
