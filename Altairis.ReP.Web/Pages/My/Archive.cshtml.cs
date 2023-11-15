using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Altairis.ReP.Web.Pages.My;
public class ArchiveModel(RepDbContext dc, UserManager<ApplicationUser> userManager) : PageModel {
    private readonly RepDbContext dc = dc;
    private readonly UserManager<ApplicationUser> userManager = userManager;

    public IEnumerable<ReservationInfo> Reservations { get; set; } = Enumerable.Empty<ReservationInfo>();

    public class ReservationInfo {

        public int Id { get; set; }

        public int ResourceId { get; set; }

        public string ResourceName { get; set; } = string.Empty;

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool System { get; set; }

    }

    public async Task OnGetAsync() {
        var userId = int.Parse(this.userManager.GetUserId(this.User) ?? throw new ImpossibleException());
        var reservationsQuery = from r in this.dc.Reservations
                                where r.UserId == userId
                                orderby r.DateBegin descending
                                select new ReservationInfo {
                                    DateBegin = r.DateBegin,
                                    DateEnd = r.DateEnd,
                                    Id = r.Id,
                                    ResourceId = r.ResourceId,
                                    ResourceName = r.Resource!.Name,
                                    System = r.System
                                };
        this.Reservations = await reservationsQuery.ToListAsync();
    }
}
