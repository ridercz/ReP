using Altairis.Services.DateProvider;
using Altairis.TagHelpers;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Altairis.ReP.Web.Pages.My;

public partial class CalendarModel : PageModel
{
    private readonly IReservationService _reservationService;
    private readonly IResourceService _resourceService;
    private readonly ICalendarEntryService _calendarEntryService;
    private readonly IMapper _mapper;

    private readonly IDateProvider dateProvider;
    private readonly IOptions<AppSettings> options;

    public CalendarModel(IReservationService reservationService, IResourceService resourceService, ICalendarEntryService calendarEntryService, IMapper mapper, IDateProvider dateProvider, IOptions<AppSettings> options)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
        _calendarEntryService = calendarEntryService ?? throw new ArgumentNullException(nameof(calendarEntryService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public bool CanManageEntries => this.options.Value.Features.UseCalendarEntries && this.User.IsPrivilegedUser();

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today.AddDays(1);

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [DataType("Markdown")]
        public string Comment { get; set; }

    }

    public IEnumerable<CalendarEvent> Reservations { get; set; }

    public IEnumerable<ResourceTag> Resources { get; set; }

    public IEnumerable<CalendarEntryInfo> CalendarEntries { get; set; }

    public class CalendarEntryInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }

    public class ResourceTag
    {
        public string Name { get; set; } = String.Empty;
        public string ForegroundColor { get; set; } = String.Empty;
        public string BackgroundColor { get; set; } = String.Empty;
        public string GetStyle() => $"color:{this.ForegroundColor};background-color:{this.BackgroundColor};";
    }

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public DateTime DatePrev { get; set; }

    public DateTime DateNext { get; set; }

    public async Task<IActionResult> OnGetAsync(int? year, int? month, CancellationToken token)
    {
        // Redirect to current month
        if (!year.HasValue || !month.HasValue) return this.RedirectToPage(new { this.dateProvider.Today.Year, this.dateProvider.Today.Month });

        // Initialize data
        await Init(year, month, token);
        return this.Page();
    }

    public async Task<IActionResult> OnGetDeleteAsync(int? year, int? month, int entryId, CancellationToken token)
    {
        // Initialize data
        await Init(year, month, token);

        // Delete entry
        if (this.CanManageEntries)
        {
            await _calendarEntryService.DeleteCalendarEntryByIdAsync(entryId, token);
        }

        return this.RedirectToPage(pageName: null, pageHandler: null, fragment: string.Empty);
    }

    public async Task<IActionResult> OnPostAsync(int? year, int? month, CancellationToken token)
    {
        // Initialize data
        await Init(year, month,token);

        // Validate arguments
        if (!this.ModelState.IsValid || !this.CanManageEntries) return this.Page();

        // Create new entry
        await _calendarEntryService.CreateCalendarEntryAsync(Input.Date, Input.Title, Input.Comment, token);


        return this.RedirectToPage(pageName: null, pageHandler: null, fragment: string.Empty);
    }

    private async Task Init(int? year, int? month, CancellationToken token)
    {
        // Get month name for display
        this.DateBegin = new DateTime(year.Value, month.Value, 1);
        this.DateEnd = this.DateBegin.AddMonths(1).AddDays(-1);
        this.DatePrev = this.DateBegin.AddMonths(-1);
        this.DateNext = this.DateBegin.AddMonths(+1);

        // Get all resources for tags
        this.Resources = _mapper.Map<IEnumerable<ResourceTag>>(await _resourceService.GetResourceTagsAsync(token));

        var qri = (await _reservationService.GetReservationsBetweenAsync(this.DateBegin.AddDays(-6), this.DateEnd.AddDays(6),token))
              .Select(rwdid => new CalendarEvent
              {
                  Id = "reservation_" + rwdid.Id,
                  BackgroundColor = rwdid.System ? rwdid.ResourceForegroundColor : rwdid.ResourceBackgroundColor,
                  ForegroundColor = rwdid.System ? rwdid.ResourceBackgroundColor : rwdid.ResourceForegroundColor,
                  CssClass = rwdid.System ? "system" : string.Empty,
                  DateBegin = rwdid.DateBegin,
                  DateEnd = rwdid.DateEnd,
                  Name = rwdid.System ? rwdid.Comment : rwdid.UserDisplayName,
                  Description = rwdid.System ? rwdid.UserDisplayName : rwdid.Comment,
                  IsFullDay = false
              });

        var calendarEntries = await _calendarEntryService.GetCalendarEntriesBetweenAsync(DateBegin.AddDays(-6), DateEnd.AddDays(6),token);

        var qei = calendarEntries.Select(ce => new CalendarEvent
        {
            Id = "event_" + ce.Id,
            BackgroundColor = this.options.Value.Design.CalendarEntryBgColor,
            ForegroundColor = this.options.Value.Design.CalendarEntryFgColor,
            DateBegin = ce.Date,
            DateEnd = ce.Date,
            Name = ce.Title,
            IsFullDay = true,
            Href = "#event_detail_" + ce.Id,
        });

        this.Reservations = qei.Concat(qri);

        this.CalendarEntries = calendarEntries.Select(ce => new CalendarEntryInfo
        {
            Id = ce.Id,
            Date = ce.Date,
            Title = ce.Title,
            Comment = ce.Comment
        });
    }

}
