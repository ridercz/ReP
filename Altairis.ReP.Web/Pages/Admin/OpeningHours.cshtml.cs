using Altairis.ReP.Data.Entities;
using Altairis.ValidationToolkit;

namespace Altairis.ReP.Web.Pages.Admin;
public class OpeningHoursModel : PageModel
{
    private readonly IOpeningHoursChangeService _service;

    private readonly OpeningHoursProvider hoursProvider;

    public OpeningHoursModel(IOpeningHoursChangeService service, OpeningHoursProvider hoursProvider)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        this.hoursProvider = hoursProvider ?? throw new ArgumentNullException(nameof(hoursProvider));
    }

    public IEnumerable<OpeningHoursInfo> StandardOpeningHours => this.hoursProvider.GetStandardOpeningHours();

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public IEnumerable<OpeningHoursChange> OpeningHoursChanges { get; set; }

    public class InputModel
    {

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today.AddDays(1);

        [DataType(DataType.Time), Range(typeof(TimeSpan), "00:00:00", "23:59:59")]
        public TimeSpan OpeningTime { get; set; } = TimeSpan.Zero;

        [DataType(DataType.Time), Range(typeof(TimeSpan), "00:00:00", "23:59:59"), GreaterThan(nameof(OpeningTime), AllowEqual = true)]
        public TimeSpan ClosingTime { get; set; } = TimeSpan.Zero;

    }

    public async Task OnGetAsync(CancellationToken token) => this.OpeningHoursChanges = await _service.GetOpeningHoursChangesAsync(token);

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!this.ModelState.IsValid) return this.Page();

        await _service.SaveOpeningHoursChangeAsync(this.Input.Date, this.Input.OpeningTime, this.Input.ClosingTime, token);

        return this.RedirectToPage(string.Empty, null, "created");
    }

    public async Task<IActionResult> OnGetDeleteAsync(int ohchId, CancellationToken token)
    {
        if (await _service.DeleteOpeningHoursChangeAsync(ohchId, token) == CommandStatus.NotFound) return this.NotFound();

        return this.RedirectToPage(string.Empty, null, "deleted");
    }

}
