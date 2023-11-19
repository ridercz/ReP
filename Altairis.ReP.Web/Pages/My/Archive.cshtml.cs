using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My;

public class ArchiveModel(RepDbContext dc, UserManager<ApplicationUser> userManager) : PageModel {

    // Output model

    public IEnumerable<ReservationInfo> Reservations { get; set; } = Enumerable.Empty<ReservationInfo>();

    public class ReservationInfo {

        public int Id { get; set; }

        public int ResourceId { get; set; }

        public string ResourceName { get; set; } = string.Empty;

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool System { get; set; }

    }

    // Handlers

    public async Task OnGetAsync() {
        var userId = int.Parse(userManager.GetUserId(this.User) ?? throw new ImpossibleException());
        var reservationsQuery = from r in dc.Reservations
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
