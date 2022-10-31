using Altairis.ValidationToolkit;

namespace Altairis.ReP.Web.Pages.Admin.Resources;
public class CreateModel : PageModel
{
    private readonly IResourceService _service;

    public CreateModel(IResourceService service)
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType("Markdown")]
        public string Instructions { get; set; }

        [Required, Range(0, 1440)]
        public int MaximumReservationTime { get; set; }

        [Required, Color]
        public string ForegroundColor { get; set; } = "#000000";

        [Required, Color]
        public string BackgroundColor { get; set; } = "#ffffff";

        public bool ResourceEnabled { get; set; } = true;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!this.ModelState.IsValid) return this.Page();

        await _service.SaveAsync(Input.Name,
                                 Input.Description,
                                 Input.Instructions,
                                 Input.MaximumReservationTime,
                                 Input.ResourceEnabled,
                                 Input.ForegroundColor,
                                 Input.BackgroundColor,
                                 token);

        return this.RedirectToPage("Index", null, "created");
    }
}
