using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My;
public class ArchiveModel : PageModel {
    private readonly RepDbContext dc;
    private readonly UserManager<ApplicationUser> userManager;

    public ArchiveModel(RepDbContext dc, UserManager<ApplicationUser> userManager) {
        this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public IEnumerable<ReservationInfo> Reservations { get; set; }

    public class ReservationInfo {

        public int Id { get; set; }

        public int ResourceId { get; set; }

        public string ResourceName { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool System { get; set; }

    }

    public async Task OnGetAsync() {
        var userId = int.Parse(this.userManager.GetUserId(this.User));
        var reservationsQuery = from r in this.dc.Reservations
                                where r.UserId == userId
                                orderby r.DateBegin descending
                                select new ReservationInfo {
                                    DateBegin = r.DateBegin,
                                    DateEnd = r.DateEnd,
                                    Id = r.Id,
                                    ResourceId = r.ResourceId,
                                    ResourceName = r.Resource.Name,
                                    System = r.System
                                };
        this.Reservations = await reservationsQuery.ToListAsync();
    }
}
