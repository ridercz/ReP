using Altairis.Services.DateProvider;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My;
public class IndexModel(RepDbContext dc, UserManager<ApplicationUser> userManager, IDateProvider dateProvider, OpeningHoursProvider hoursProvider) : PageModel {
    private readonly RepDbContext dc = dc;
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly IDateProvider dateProvider = dateProvider;
    private readonly OpeningHoursProvider hoursProvider = hoursProvider;

    public IEnumerable<Resource> Resources { get; set; } = Enumerable.Empty<Resource>();

    public IEnumerable<ReservationInfo> Reservations { get; set; } = Enumerable.Empty<ReservationInfo>();

    public OpeningHoursInfo? OpenToday { get; set; }

    public OpeningHoursInfo? OpenTomorrow { get; set; }

    public DateTime LastNewsDate { get; set; }

    public string? LastNewsTitle { get; set; }

    public string? LastNewsText { get; set; }

    public record ReservationInfo(int Id, int ResourceId, string ResourceName, DateTime DateBegin, DateTime DateEnd, bool CanBeDeleted);

    public async Task OnGetAsync() {
        // Get operning hours
        this.OpenToday = this.hoursProvider.GetOpeningHours(0);
        this.OpenTomorrow = this.hoursProvider.GetOpeningHours(1);

        // Get latest news message
        var latestNews = await this.dc.NewsMessages.OrderByDescending(x => x.Date).FirstOrDefaultAsync();
        if (latestNews != null) {
            this.LastNewsDate = latestNews.Date;
            this.LastNewsTitle = latestNews.Title;
            this.LastNewsText = latestNews.Text;
        }

        // Get resources accessible to user
        var resourcesQuery = this.dc.Resources.OrderBy(x => x.Name);
        if (!this.User.IsPrivilegedUser()) resourcesQuery = (IOrderedQueryable<Resource>)resourcesQuery.Where(x => x.Enabled);
        this.Resources = await resourcesQuery.ToListAsync();

        // Get reservations of this user
        var userId = int.Parse(this.userManager.GetUserId(this.User) ?? throw new ImpossibleException());
        var now = this.dateProvider.Now;
        var reservationsQuery = from r in this.dc.Reservations
                                where r.UserId == userId && r.DateEnd >= this.dateProvider.Today
                                orderby r.DateBegin
                                select new ReservationInfo(r.Id, r.ResourceId, r.Resource!.Name, r.DateBegin, r.DateEnd, r.DateEnd > this.dateProvider.Now);
        this.Reservations = await reservationsQuery.ToListAsync();
    }

    public async Task<IActionResult> OnGetDeleteAsync(int reservationId) {
        var userId = int.Parse(this.userManager.GetUserId(this.User) ?? throw new ImpossibleException());
        var reservation = await this.dc.Reservations.SingleOrDefaultAsync(x => x.Id == reservationId && x.UserId == userId && x.DateEnd > this.dateProvider.Now);
        if (reservation == null) return this.NotFound();
        this.dc.Reservations.Remove(reservation);
        await this.dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "reservationdeleted");
    }

    public async Task<IActionResult> OnGetSaveIcsAsync(int reservationId) {
        var userId = int.Parse(this.userManager.GetUserId(this.User) ?? throw new ImpossibleException());
        var reservation = await this.dc.Reservations.Include(x => x.Resource).SingleOrDefaultAsync(x => x.Id == reservationId && x.UserId == userId);
        if (reservation == null) return this.NotFound();

        // Create ICS
        var cal = new Calendar();
        cal.Events.Add(new() {
            Summary = reservation.Resource!.Name,
            Description = reservation.Comment,
            DtStart = new CalDateTime(reservation.DateBegin),
            DtEnd = new CalDateTime(reservation.DateEnd),
            Uid = this.Url.PageLink(pageName: "Calendar", protocol: this.Request.Scheme, fragment: "#reservation_" + reservation.Id)
        });

        // Serialize to ICAL
        var calSer = new CalendarSerializer(cal);
        var ics = calSer.SerializeToString();
        return this.Content(ics, "text/calendar");
    }

}
