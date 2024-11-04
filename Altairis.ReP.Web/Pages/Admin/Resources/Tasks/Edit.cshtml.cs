
namespace Altairis.ReP.Web.Pages.Admin.Resources.Tasks;

public class EditModel(RepDbContext dc) : PageModel {
    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Interval { get; set; } = string.Empty;

        [DataType("Markdown")]
        public string? Description { get; set; }

    }

    // Output model

    public string ResourceName { get; set; } = string.Empty;

    // Handlers

    public async Task<IActionResult> OnGetAsync(int resourceId, int taskId) {
        if (!await this.InitAsync(resourceId)) return this.NotFound();

        // Get task data
        var task = await dc.MaintenanceTasks.FindAsync(taskId);
        if (task == null) return this.NotFound();

        // Fill input model
        this.Input = new InputModel {
            Name = task.Name,
            Description = task.Description,
            Interval = task.Interval
        };

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int resourceId, int taskId) {
        if (!await this.InitAsync(resourceId)) return this.NotFound();
        if (!this.ModelState.IsValid) return this.Page();

        // Get task data
        var task = await dc.MaintenanceTasks.FindAsync(taskId);
        if (task == null) return this.NotFound();

        // Update task data
        task.Name = this.Input.Name;
        task.Description = this.Input.Description;
        task.Interval = this.Input.Interval;

        await dc.SaveChangesAsync();
        return this.RedirectToPage("Index", new { resourceId });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int resourceId, int taskId) {
        if (!await this.InitAsync(resourceId)) return this.NotFound();

        // Get task data
        var task = await dc.MaintenanceTasks.FindAsync(taskId);
        if (task != null) {

            // Delete task
            dc.MaintenanceTasks.Remove(task);
            await dc.SaveChangesAsync();
        }

        return this.RedirectToPage("Index", new { resourceId });
    }

    // Helpers

    public async Task<bool> InitAsync(int resourceId) {
        var resource = await dc.Resources.FindAsync(resourceId);
        if (resource == null) return false;
        this.ResourceName = resource.Name;
        return true;
    }

}
