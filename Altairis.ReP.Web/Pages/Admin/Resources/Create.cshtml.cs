using Altairis.ValidationToolkit;

namespace Altairis.ReP.Web.Pages.Admin.Resources; 
public class CreateModel : PageModel {
    private readonly RepDbContext dc;

    public CreateModel(RepDbContext dc) {
        this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required, Range(0, 1440)]
        public int MaximumReservationTime { get; set; }

        [Required, Color]
        public string ForegroundColor { get; set; } = "#000000";

        [Required, Color]
        public string BackgroundColor { get; set; } = "#ffffff";

        public bool ResourceEnabled { get; set; } = true;

    }

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        var newResource = new Resource {
            Description = this.Input.Description,
            Enabled = this.Input.ResourceEnabled,
            MaximumReservationTime = this.Input.MaximumReservationTime,
            Name = this.Input.Name,
            ForegroundColor = this.Input.ForegroundColor,
            BackgroundColor = this.Input.BackgroundColor
        };
        this.dc.Resources.Add(newResource);
        await this.dc.SaveChangesAsync();

        return this.RedirectToPage("Index", null, "created");
    }

}
