using Altairis.ReP.Web.Pages.Models;
using Altairis.Services.DateProvider;
using Altairis.TagHelpers;
using Microsoft.Extensions.Options;

namespace Altairis.ReP.Web.Pages.My;

public class CalendarModel : PageModel {
    private readonly RepDbContext dc;
    private readonly IDateProvider dateProvider;
    private readonly IOptions<AppSettings> options;

    public CalendarModel(RepDbContext dc, IDateProvider dateProvider, IOptions<AppSettings> options) {
        this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today.AddDays(1);

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [DataType("Markdown")]
        public string Comment { get; set; }

    }

    public bool CanManageEntries => this.options.Value.Features.UseCalendarEntries && this.User.IsPrivilegedUser();

    public IEnumerable<ResourceTag> Resources { get; set; }

    public class ResourceTag {
        public string Name { get; set; }
        public string ForegroundColor { get; set; }
        public string BackgroundColor { get; set; }

        public string GetStyle() => $"color:{this.ForegroundColor};background-color:{this.BackgroundColor};";
    }

    public IEnumerable<CalendarEvent> Reservations { get; set; }

    public IEnumerable<CalendarEntryInfo> CalendarEntries { get; set; }

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public DateTime DatePrev { get; set; }

    public DateTime DateNext { get; set; }

    public async Task<IActionResult> OnGetAsync(int? year, int? month) {
        // Redirect to current month
        if (!year.HasValue || !month.HasValue) return this.RedirectToPage(new { this.dateProvider.Today.Year, this.dateProvider.Today.Month });

        // Initialize data
        await Init(year, month);
        return this.Page();
    }

    public async Task<IActionResult> OnGetDeleteAsync(int? year, int? month, int entryId) {
        // Initialize data
        await Init(year, month);

        // Delete entry
        if (this.CanManageEntries) {
            this.dc.CalendarEntries.Remove(new CalendarEntry { Id = entryId });
            await this.dc.SaveChangesAsync();
        }
        return this.RedirectToPage(pageName: null, pageHandler: null, fragment: string.Empty);
    }

    public async Task<IActionResult> OnPostAsync(int? year, int? month) {
        // Initialize data
        await Init(year, month);

        // Validate arguments
        if (!this.ModelState.IsValid || !this.CanManageEntries) return this.Page();

        // Create new entry
        await this.dc.CalendarEntries.AddAsync(new CalendarEntry {
            Date = this.Input.Date,
            Comment = this.Input.Comment,
            Title = this.Input.Title
        });
        await this.dc.SaveChangesAsync();
        return this.RedirectToPage(pageName: null, pageHandler: null, fragment: string.Empty);
    }

    private async Task Init(int? year, int? month) {
        // Get month name for display
        this.DateBegin = new DateTime(year.Value, month.Value, 1);
        this.DateEnd = this.DateBegin.AddMonths(1).AddDays(-1);
        this.DatePrev = this.DateBegin.AddMonths(-1);
        this.DateNext = this.DateBegin.AddMonths(+1);

        // Get all resources for tags
        this.Resources = await this.dc.Resources
            .OrderBy(x => x.Name)
            .Select(x => new ResourceTag {
                Name = x.Name,
                ForegroundColor = x.ForegroundColor,
                BackgroundColor = x.BackgroundColor
            })
            .ToListAsync();

        // Get all reservations in this month
        var qr = from r in this.dc.Reservations
                 where r.DateEnd >= this.DateBegin.AddDays(-6) && r.DateBegin < this.DateEnd.AddDays(6)
                 orderby r.DateBegin
                 select new CalendarEvent {
                     Id = "reservation_" + r.Id,
                     BackgroundColor = r.System ? r.Resource.ForegroundColor : r.Resource.BackgroundColor,
                     ForegroundColor = r.System ? r.Resource.BackgroundColor : r.Resource.ForegroundColor,
                     CssClass = r.System ? "system" : string.Empty,
                     DateBegin = r.DateBegin,
                     DateEnd = r.DateEnd,
                     Name = r.System ? r.Comment : r.User.DisplayName,
                     Description = r.System ? r.User.DisplayName : r.Comment,
                     IsFullDay = false
                 };

        // Get all calendar entries in this month
        var qe = from e in this.dc.CalendarEntries
                 where e.Date >= this.DateBegin.AddDays(-6) && e.Date < this.DateEnd.AddDays(6)
                 orderby e.Date
                 select new CalendarEvent {
                     Id = "event_" + e.Id,
                     BackgroundColor = this.options.Value.Design.CalendarEntryBgColor,
                     ForegroundColor = this.options.Value.Design.CalendarEntryFgColor,
                     DateBegin = e.Date,
                     DateEnd = e.Date,
                     Name = e.Title,
                     IsFullDay = true,
                     Href = "#event_detail_" + e.Id,
                 };
        var qd = from e in this.dc.CalendarEntries
                 where e.Date >= this.DateBegin.AddDays(-6) && e.Date < this.DateEnd.AddDays(6)
                 orderby e.Date
                 select new CalendarEntryInfo {
                     Id = e.Id,
                     Date = e.Date,
                     Title = e.Title,
                     Comment = e.Comment
                 };

        // Materialize queries
        var qri = await qr.ToListAsync();
        var qei = await qe.ToListAsync();
        this.Reservations = qei.Concat(qri);
        this.CalendarEntries = await qd.ToListAsync();
    }

}
