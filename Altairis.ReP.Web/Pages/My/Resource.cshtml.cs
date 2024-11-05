using System.Globalization;
using Altairis.TagHelpers;
using Altairis.ValidationToolkit;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My;

public class ResourceModel(RepDbContext dc, TimeProvider timeProvider, UserManager<ApplicationUser> userManager, OpeningHoursProvider hoursProvider, IOptions<AppSettings> options, ResourceAttachmentProcessor attachmentProcessor) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [DataType(DataType.DateTime), DateOffset(0, 1, CompareTime = true)]
        public DateTime DateBegin { get; set; }

        [DataType(DataType.DateTime), GreaterThan(nameof(DateBegin))]
        public DateTime DateEnd { get; set; }

        public bool System { get; set; }

        public string? Comment { get; set; }
    }

    // Output model

    public bool CanManageEntries => options.Value.Features.UseCalendarEntries && this.User.IsPrivilegedUser();

    public Resource? Resource { get; set; }

    public IEnumerable<CalendarEvent> Reservations { get; set; } = [];

    public IEnumerable<CalendarEntryInfo> CalendarEntries { get; set; } = [];

    public record CalendarEntryInfo(int Id, DateTime Date, string? Title, string? Comment);

    public DateTime CalendarDateBegin { get; set; }

    public DateTime CalendarDateEnd { get; set; }

    public IEnumerable<ResourceAttachment> Attachments { get; set; } = [];

    public bool CanDoReservation { get; set; } = false;

    public string ResourceAuthorizationKey { get; set; } = string.Empty;

    public bool MaintenanceTasksDefined { get; set; }

    // Handlers

    public async Task<IActionResult> OnGetAsync(int resourceId) {
        if (!await this.Init(resourceId)) return this.NotFound();

        var dt = timeProvider.GetLocalNow().Date.AddDays(1);
        this.Input.DateBegin = dt.AddMinutes(-dt.Minute);
        this.Input.DateEnd = this.Input.DateBegin.AddHours(1);

        return this.Page();
    }

    public async Task<IActionResult> OnGetDownload(int attachmentId) {
        if (!options.Value.Features.UseAttachments) return this.NotFound();
        try {
            var result = await attachmentProcessor.GetAttachment(attachmentId);
            return this.File(result.Item1, "application/octet-stream", result.Item2);
        } catch (FileNotFoundException) {
            return this.NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync(int resourceId) {
        if (!(await this.Init(resourceId) && (this.Resource!.Enabled || this.User.IsPrivilegedUser()))) return this.NotFound();
        if (!this.ModelState.IsValid) return this.Page();

        if (!this.User.IsPrivilegedUser()) {
            // Check reservation time length
            var resLength = this.Input.DateEnd.Subtract(this.Input.DateBegin).TotalMinutes;
            if (this.Resource.MaximumReservationTime > 0 && resLength > this.Resource.MaximumReservationTime) {
                this.ModelState.AddModelError(string.Empty, string.Format(UI.My_Reservations_Err_Maxlength, this.Resource.MaximumReservationTime));
                return this.Page();
            }

            // Check if it begins and ends in the same day
            if (options.Value.Features.UseOpeningHours && this.Input.DateBegin.Date != this.Input.DateEnd.Date) {
                this.ModelState.AddModelError(string.Empty, UI.My_Reservations_Err_SingleDay);
                return this.Page();
            }

            // Check against opening times
            if (options.Value.Features.UseOpeningHours) {
                var openTime = hoursProvider.GetOpeningHours(this.Input.DateBegin);
                if (this.Input.DateBegin < openTime.AbsoluteOpeningTime || this.Input.DateEnd > openTime.AbsoluteClosingTime) {
                    this.ModelState.AddModelError(string.Empty, UI.My_Reservations_Err_OpeningHours);
                    return this.Page();
                }
            }
        }

        // Check against other reservations
        var q = from r in dc.Reservations
                where r.ResourceId == resourceId && r.DateBegin < this.Input.DateEnd && r.DateEnd > this.Input.DateBegin
                select new { r.DateBegin, r.User!.UserName };
        foreach (var item in await q.ToListAsync()) {
            this.ModelState.AddModelError(string.Empty, string.Format(UI.My_Reservations_Err_Conflict, item.UserName, item.DateBegin));
        }
        if (!this.ModelState.IsValid) return this.Page();

        // Create reservation
        var newReservation = new Reservation {
            DateBegin = this.Input.DateBegin,
            DateEnd = this.Input.DateEnd,
            UserId = int.Parse(userManager.GetUserId(this.User) ?? throw new ImpossibleException()),
            ResourceId = resourceId,
            System = this.User.IsPrivilegedUser() && this.Input.System,
            Comment = this.User.IsPrivilegedUser() ? this.Input.Comment : null

        };
        dc.Reservations.Add(newReservation);
        await dc.SaveChangesAsync();

        return this.RedirectToPage("Reservations", string.Empty, new { resourceId }, "created");
    }

    // Helpers

    private async Task<bool> Init(int resourceId) {
        // Get resource
        this.Resource = await dc.Resources.Include(x => x.MaintenanceTasks).SingleOrDefaultAsync(x => x.Id == resourceId) ?? throw new ImpossibleException();
        if (this.Resource == null) return false;
        this.CanDoReservation = this.Resource.Enabled || this.User.IsPrivilegedUser();

        // Get user RAK
        this.ResourceAuthorizationKey = (await dc.Users.SingleAsync(x => x.UserName!.Equals(this.User.Identity!.Name))).ResourceAuthorizationKey;

        // Get last Monday as the start date
        this.CalendarDateBegin = timeProvider.GetLocalNow().Date;
        while (this.CalendarDateBegin.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) this.CalendarDateBegin = this.CalendarDateBegin.AddDays(-1);

        // Get future reservations
        var qr = from r in dc.Reservations
                 where r.ResourceId == resourceId && r.DateBegin >= this.CalendarDateBegin
                 select new CalendarEvent {
                     Id = "reservation_" + r.Id,
                     BackgroundColor = r.System ? r.Resource!.ForegroundColor : r.Resource!.BackgroundColor,
                     ForegroundColor = r.System ? r.Resource.BackgroundColor : r.Resource.ForegroundColor,
                     CssClass = r.System ? "system" : string.Empty,
                     DateBegin = r.DateBegin,
                     DateEnd = r.DateEnd,
                     Name = r.System ? r.Comment : r.User!.DisplayName,
                     Description = r.System ? r.User!.DisplayName : r.Comment,
                     IsFullDay = false,
                 };

        // Get Future calendar entries
        var qe = from e in dc.CalendarEntries
                 where e.Date >= this.CalendarDateBegin
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
                 where e.Date >= this.CalendarDateBegin
                 orderby e.Date
                 select new CalendarEntryInfo(e.Id, e.Date, e.Title, e.Comment);

        // Materialize queries
        var qri = await qr.ToListAsync();
        var qei = await qe.ToListAsync();
        this.Reservations = qei.Concat(qri);
        this.CalendarEntries = await qd.ToListAsync();

        // Get calendar end date
        var lastEventEnd = this.Reservations.Max(x => x.DateEnd);
        this.CalendarDateEnd = this.CalendarDateBegin.AddMonths(1);
        if (lastEventEnd.HasValue && lastEventEnd > this.CalendarDateEnd) this.CalendarDateEnd = lastEventEnd.Value;

        // Get attachments
        if (options.Value.Features.UseAttachments) this.Attachments = await dc.ResourceAttachments.Where(x => x.ResourceId == resourceId).OrderByDescending(x => x.DateCreated).ToListAsync();

        // Get maintenance tasks
        this.MaintenanceTasksDefined = this.Resource.MaintenanceTasks.Any();

        return true;
    }

}

