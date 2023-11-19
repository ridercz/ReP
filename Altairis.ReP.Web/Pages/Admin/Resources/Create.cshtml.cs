using Altairis.ValidationToolkit;

namespace Altairis.ReP.Web.Pages.Admin.Resources;

public class CreateModel(RepDbContext dc) : PageModel {

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

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        var newResource = new Resource {
            Description = this.Input.Description,
            Enabled = this.Input.ResourceEnabled,
            Instructions = this.Input.Instructions,
            MaximumReservationTime = this.Input.MaximumReservationTime,
            Name = this.Input.Name,
            ForegroundColor = this.Input.ForegroundColor,
            BackgroundColor = this.Input.BackgroundColor
        };
        dc.Resources.Add(newResource);
        await dc.SaveChangesAsync();

        return this.RedirectToPage("Index", null, "created");
    }

}
