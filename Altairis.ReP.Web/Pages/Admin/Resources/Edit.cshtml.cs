using Altairis.ValidationToolkit;

namespace Altairis.ReP.Web.Pages.Admin.Resources;

public class EditModel(RepDbContext dc) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [DataType("Markdown")]
        public string? Instructions { get; set; }

        [Required, Range(0, 1440)]
        public int MaximumReservationTime { get; set; }

        [Required, Color]
        public string ForegroundColor { get; set; } = "#000000";

        [Required, Color]
        public string BackgroundColor { get; set; } = "#ffffff";

        public bool ResourceEnabled { get; set; } = true;

    }

    // Handlers

    public async Task<IActionResult> OnGetAsync(int resourceId) {
        var resource = await dc.Resources.FindAsync(resourceId);
        if (resource == null) return this.NotFound();

        this.Input = new InputModel {
            Description = resource.Description,
            ResourceEnabled = resource.Enabled,
            MaximumReservationTime = resource.MaximumReservationTime,
            Name = resource.Name,
            ForegroundColor = resource.ForegroundColor,
            BackgroundColor = resource.BackgroundColor,
            Instructions = resource.Instructions
        };
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int resourceId) {
        var resource = await dc.Resources.FindAsync(resourceId);
        if (resource == null) return this.NotFound();

        if (!this.ModelState.IsValid) return this.Page();

        resource.Description = this.Input.Description;
        resource.Enabled = this.Input.ResourceEnabled;
        resource.MaximumReservationTime = this.Input.MaximumReservationTime;
        resource.Name = this.Input.Name;
        resource.ForegroundColor = this.Input.ForegroundColor;
        resource.BackgroundColor = this.Input.BackgroundColor;
        resource.Instructions = this.Input.Instructions;

        await dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "saved");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int resourceId) {
        var resource = await dc.Resources.FindAsync(resourceId);
        if (resource == null) return this.NotFound();

        dc.Resources.Remove(resource);

        await dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "deleted");
    }
}
