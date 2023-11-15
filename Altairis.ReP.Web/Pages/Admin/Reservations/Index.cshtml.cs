namespace Altairis.ReP.Web.Pages.Admin.Reservations;

public class IndexModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc;

    public record ReservationInfo(int ReservationId, string UserDisplayName, string ResourceName, DateTime DateBegin, DateTime DateEnd, bool System, string? Comment);

    public IEnumerable<ReservationInfo> Reservations { get; set; } = new List<ReservationInfo>();

    public async Task OnGetAsync() {
        var q = from r in this.dc.Reservations
                orderby r.DateBegin descending
                select new ReservationInfo(r.Id, r.User!.DisplayName, r.Resource!.Name, r.DateBegin, r.DateEnd, r.System, r.Comment);
        this.Reservations = await q.ToListAsync();
    }

}
