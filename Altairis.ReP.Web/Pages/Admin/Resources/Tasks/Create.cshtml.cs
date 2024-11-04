namespace Altairis.ReP.Web.Pages.Admin.Resources.Tasks;

public class CreateModel(RepDbContext dc) : PageModel {

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

    public async Task<IActionResult> OnGetAsync(int resourceId) => await this.InitAsync(resourceId) ? this.Page() : this.NotFound();

    public async Task<IActionResult> OnPostAsync(int resourceId) {
        if (!await this.InitAsync(resourceId)) return this.NotFound();
        if (!this.ModelState.IsValid) return this.Page();

        var newTask = new MaintenanceTask {
            Name = this.Input.Name,
            Interval = this.Input.Interval,
            Description = this.Input.Description,
            ResourceId = resourceId
        };
        dc.MaintenanceTasks.Add(newTask);
        await dc.SaveChangesAsync();
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
