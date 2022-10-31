using Altairis.ValidationToolkit;

namespace Altairis.ReP.Web.Pages.Admin.Resources;
public class EditModel : PageModel
{
    private readonly IResourceService _service;

    public EditModel(IResourceService service) 
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

    public async Task<IActionResult> OnGetAsync(int resourceId, CancellationToken token)
    {
        var resource = await _service.GetResourceByIdOrNullAsync(resourceId, token);
        if (resource == null) return this.NotFound();

        this.Input = new InputModel
        {
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

    public async Task<IActionResult> OnPostAsync(int resourceId, CancellationToken token)
        => await _service.SaveAsync(resourceId,
                                      Input.Name,
                                      Input.Description,
                                      Input.Instructions,
                                      Input.MaximumReservationTime,
                                      Input.ResourceEnabled,
                                      Input.ForegroundColor,
                                      Input.BackgroundColor,
                                      token) == CommandStatus.NotFound
            ? this.NotFound()
            : this.RedirectToPage("Index", null, "saved");


    public async Task<IActionResult> OnPostDeleteAsync(int resourceId)
        => await _service.DeleteAsync(resourceId) == CommandStatus.NotFound
            ? this.NotFound()
            : this.RedirectToPage("Index", null, "deleted");
}