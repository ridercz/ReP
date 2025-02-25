using Altairis.TagHelpers;
namespace Altairis.ReP.Web.Pages.My;

public class CalendarModel(RepDbContext dc, TimeProvider timeProvider, IOptions<AppSettings> options) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today.AddDays(1);

        [Required, MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [DataType("Markdown")]
        public string? Comment { get; set; }

    }

    // Output model

    public bool CanManageEntries => options.Value.Features.UseCalendarEntries && this.User.IsPrivilegedUser();

    public IEnumerable<ResourceTag> Resources { get; set; } = [];

    public record ResourceTag(string Name, string ForegroundColor, string BackgroundColor) {
        public string GetStyle() => $"color:{this.ForegroundColor};background-color:{this.BackgroundColor};";
    }

    public IEnumerable<CalendarEvent> Reservations { get; set; } = [];

    public IEnumerable<CalendarEntryInfo> CalendarEntries { get; set; } = [];

    public record CalendarEntryInfo(int Id, DateTime Date, string? Title, string? Comment);

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public DateTime DatePrev { get; set; }

    public DateTime DateNext { get; set; }

    public string ResourceAuthorizationKey { get; set; } = string.Empty;

    // Handlers

    public async Task<IActionResult> OnGetAsync(int? year, int? month) {
        // Redirect to current month
        if (!year.HasValue || !month.HasValue) return this.RedirectToPage(new { timeProvider.GetLocalNow().Year, timeProvider.GetLocalNow().Month });

        // Initialize data
        await this.Init(year.Value, month.Value);
        return this.Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int year, int month, int entryId) {
        // Initialize data
        await this.Init(year, month);

        // Delete entry
        if (this.CanManageEntries) {
            dc.CalendarEntries.Remove(new CalendarEntry { Id = entryId });
            await dc.SaveChangesAsync();
        }
        return this.RedirectToPage(pageName: null, pageHandler: null, fragment: string.Empty);
    }

    public async Task<IActionResult> OnPostAsync(int year, int month) {
        // Initialize data
        await this.Init(year, month);

        // Validate arguments
        if (!this.ModelState.IsValid || !this.CanManageEntries) return this.Page();

        // Create new entry
        await dc.CalendarEntries.AddAsync(new CalendarEntry {
            Date = this.Input.Date,
            Comment = this.Input.Comment,
            Title = this.Input.Title
        });
        await dc.SaveChangesAsync();
        return this.RedirectToPage(pageName: null, pageHandler: null, fragment: string.Empty);
    }

    // Helpers

    private async Task Init(int year, int month) {
        // Get user RAK
        this.ResourceAuthorizationKey = (await dc.Users.SingleAsync(x => this.User.Identity!.Name!.Equals(x.UserName))).ResourceAuthorizationKey;

        // Get month name for display
        this.DateBegin = new DateTime(year, month, 1);
        this.DateEnd = this.DateBegin.AddMonths(1).AddDays(-1);
        this.DatePrev = this.DateBegin.AddMonths(-1);
        this.DateNext = this.DateBegin.AddMonths(+1);

        // Get all resources for tags
        this.Resources = await dc.Resources
            .OrderBy(x => x.Name)
            .Select(x => new ResourceTag(x.Name, x.ForegroundColor, x.BackgroundColor))
            .ToListAsync();

        // Get all reservations in this month
        var qr = from r in dc.Reservations
                 where r.DateEnd >= this.DateBegin.AddDays(-6) && r.DateBegin < this.DateEnd.AddDays(6)
                 orderby r.DateBegin
                 select new CalendarEvent {
                     Id = "reservation_" + r.Id,
                     BackgroundColor = r.System ? r.Resource!.ForegroundColor : r.Resource!.BackgroundColor,
                     ForegroundColor = r.System ? r.Resource.BackgroundColor : r.Resource.ForegroundColor,
                     CssClass = r.System ? "system" : string.Empty,
                     DateBegin = r.DateBegin,
                     DateEnd = r.DateEnd,
                     Name = r.System ? r.Comment : r.User!.DisplayName,
                     Description = r.System ? r.User!.DisplayName : r.Comment,
                     IsFullDay = false
                 };

        // Get all calendar entries in this month
        var qe = from e in dc.CalendarEntries
                 where e.Date >= this.DateBegin.AddDays(-6) && e.Date < this.DateEnd.AddDays(6)
                 orderby e.Date
                 select new CalendarEvent {
                     Id = "event_" + e.Id,
                     BackgroundColor = options.Value.Design.CalendarEntryBgColor,
                     ForegroundColor = options.Value.Design.CalendarEntryFgColor,
                     DateBegin = e.Date,
                     DateEnd = e.Date,
                     Name = e.Title,
                     IsFullDay = true,
                     Href = "#event_detail_" + e.Id,
                 };
        var qd = from e in dc.CalendarEntries
                 where e.Date >= this.DateBegin.AddDays(-6) && e.Date < this.DateEnd.AddDays(6)
                 orderby e.Date
                 select new CalendarEntryInfo(e.Id, e.Date, e.Title, e.Comment);

        // Get all maintenance records in this month
        var qm = from m in dc.MaintenanceRecords
                    .Include(x => x.MaintenanceTask)
                    .Include(x => x.Resource)
                    .Include(x => x.User)
                 where m.Date >= this.DateBegin.AddDays(-6) && m.Date < this.DateEnd.AddDays(6)
                 orderby m.Date
                 select new CalendarEvent {
                     Id = "maintenance_" + m.Id,
                     BackgroundColor = m.Resource!.ForegroundColor,
                     ForegroundColor = m.Resource!.BackgroundColor,
                     DateBegin = m.Date,
                     Name = $"{m.Resource.Name}: {m.MaintenanceTask.Name}",
                     Description = m.User.DisplayName,
                     CssClass = "maintenance"
                 };

        // Materialize queries
        var qri = await qr.ToListAsync();
        var qei = await qe.ToListAsync();
        var qmi = await qm.ToListAsync();
        this.Reservations = qei.Concat(qri).Concat(qmi);
        this.CalendarEntries = await qd.ToListAsync();
    }

}
