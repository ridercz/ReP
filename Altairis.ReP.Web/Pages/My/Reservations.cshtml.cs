using System.Globalization;
using Altairis.Services.DateProvider;
using Altairis.TagHelpers;
using Altairis.ValidationToolkit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Altairis.ReP.Web.Pages.My;
public class ReservationsModel : PageModel {
    private readonly RepDbContext dc;
    private readonly IDateProvider dateProvider;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly OpeningHoursProvider hoursProvider;
    private readonly AppSettings options;

    public ReservationsModel(RepDbContext dc, IDateProvider dateProvider, UserManager<ApplicationUser> userManager, OpeningHoursProvider hoursProvider, IOptions<AppSettings> optionsAccessor) {
        this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.hoursProvider = hoursProvider ?? throw new ArgumentNullException(nameof(hoursProvider));
        this.options = optionsAccessor?.Value ?? throw new ArgumentException(nameof(optionsAccessor));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [DataType(DataType.DateTime), DateOffset(0, 1, CompareTime = true)]
        public DateTime DateBegin { get; set; }

        [DataType(DataType.DateTime), GreaterThan(nameof(DateBegin))]
        public DateTime DateEnd { get; set; }

        public bool System { get; set; }

        public string Comment { get; set; }
    }

    public Resource Resource { get; set; }

    public IEnumerable<CalendarEvent> Reservations { get; set; }

    public DateTime CalendarDateBegin { get; set; }

    public DateTime CalendarDateEnd { get; set; }

    public bool CanDoReservation { get; set; } = false;

    private async Task<bool> Init(int resourceId) {
        // Get resource
        this.Resource = await this.dc.Resources.SingleOrDefaultAsync(x => x.Id == resourceId);
        if (this.Resource == null) return false;
        this.CanDoReservation = this.Resource.Enabled || this.User.IsPrivilegedUser();

        // Get last Monday as the start date
        this.CalendarDateBegin = this.dateProvider.Today;
        while (this.CalendarDateBegin.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) this.CalendarDateBegin = this.CalendarDateBegin.AddDays(-1);

        // Get future reservations
        var q = from r in this.dc.Reservations
                where r.ResourceId == resourceId && r.DateBegin >= this.CalendarDateBegin
                select new CalendarEvent {
                    Id = "reservation_" + r.Id,
                    DateBegin = r.DateBegin,
                    DateEnd = r.DateEnd,
                    Description = r.Comment,
                    IsFullDay = false,
                    Name = r.User.UserName,
                    CssClass = r.System ? "system" : string.Empty,
                    ForegroundColor = this.Resource.ForegroundColor,
                    BackgroundColor = this.Resource.BackgroundColor
                };
        var lastEventEnd = await q.MaxAsync(x => x.DateEnd);
        this.Reservations = await q.ToListAsync();
        this.CalendarDateEnd = this.CalendarDateBegin.AddMonths(1);
        if (lastEventEnd.HasValue && lastEventEnd > this.CalendarDateEnd) this.CalendarDateEnd = lastEventEnd.Value;

        return true;
    }

    public async Task<IActionResult> OnGetAsync(int resourceId) {
        if (!await this.Init(resourceId)) return this.NotFound();

        var dt = this.dateProvider.Now.AddDays(1);
        this.Input.DateBegin = dt.AddMinutes(-dt.Minute);
        this.Input.DateEnd = this.Input.DateBegin.AddHours(1);

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int resourceId) {
        if (!(await this.Init(resourceId) && (this.Resource.Enabled || this.User.IsPrivilegedUser()))) return this.NotFound();
        if (!this.ModelState.IsValid) return this.Page();

        if (!this.User.IsPrivilegedUser()) {
            // Check reservation time length
            var resLength = this.Input.DateEnd.Subtract(this.Input.DateBegin).TotalMinutes;
            if (this.Resource.MaximumReservationTime > 0 && resLength > this.Resource.MaximumReservationTime) {
                this.ModelState.AddModelError(string.Empty, string.Format(UI.My_Reservations_Err_Maxlength, this.Resource.MaximumReservationTime));
                return this.Page();
            }

            // Check if it begins and ends in the same day
            if (this.options.Features.UseOpeningHours && this.Input.DateBegin.Date != this.Input.DateEnd.Date) {
                this.ModelState.AddModelError(string.Empty, UI.My_Reservations_Err_SingleDay);
                return this.Page();
            }

            // Check against opening times
            if (this.options.Features.UseOpeningHours) {
                var openTime = this.hoursProvider.GetOpeningHours(this.Input.DateBegin);
                if (this.Input.DateBegin < openTime.AbsoluteOpeningTime || this.Input.DateEnd > openTime.AbsoluteClosingTime) {
                    this.ModelState.AddModelError(string.Empty, UI.My_Reservations_Err_OpeningHours);
                    return this.Page();
                }
            }
        }

        // Check against other reservations
        var q = from r in this.dc.Reservations
                where r.ResourceId == resourceId && r.DateBegin < this.Input.DateEnd && r.DateEnd > this.Input.DateBegin
                select new { r.DateBegin, r.User.UserName };
        foreach (var item in await q.ToListAsync()) {
            this.ModelState.AddModelError(string.Empty, string.Format(UI.My_Reservations_Err_Conflict, item.UserName, item.DateBegin));
        }
        if (!this.ModelState.IsValid) return this.Page();

        // Create reservation
        var newReservation = new Reservation {
            DateBegin = this.Input.DateBegin,
            DateEnd = this.Input.DateEnd,
            UserId = int.Parse(this.userManager.GetUserId(this.User)),
            ResourceId = resourceId,
            System = this.User.IsPrivilegedUser() && this.Input.System,
            Comment = this.User.IsPrivilegedUser() ? this.Input.Comment : null

        };
        this.dc.Reservations.Add(newReservation);
        await this.dc.SaveChangesAsync();

        return this.RedirectToPage("Reservations", string.Empty, new { resourceId }, "created");
    }

}

