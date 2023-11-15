namespace Altairis.ReP.Web.Pages.Admin.Reservations;
public class IndexModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc ?? throw new ArgumentNullException(nameof(dc));

    public class ReservationInfo {

        public int ReservationId { get; set; }

        public string UserDisplayName { get; set; }

        public string ResourceName { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool System { get; set; }

        public string Comment { get; set; }

    }

    public IEnumerable<ReservationInfo> Reservations { get; set; }

    public async Task OnGetAsync() {
        var q = from r in this.dc.Reservations
                orderby r.DateBegin descending
                select new ReservationInfo {
                    Comment = r.Comment,
                    DateBegin = r.DateBegin,
                    DateEnd = r.DateEnd,
                    ReservationId = r.Id,
                    ResourceName = r.Resource.Name,
                    System = r.System,
                    UserDisplayName = r.User.DisplayName
                };
        this.Reservations = await q.ToListAsync();
    }

}
