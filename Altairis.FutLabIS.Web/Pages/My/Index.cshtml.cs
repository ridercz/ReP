using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.Services.DateProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Web.Pages.My {
    public class IndexModel : PageModel {
        private readonly FutLabDbContext dc;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDateProvider dateProvider;

        public IndexModel(FutLabDbContext dc, UserManager<ApplicationUser> userManager, IDateProvider dateProvider) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        }

        public IEnumerable<Resource> Resources { get; set; }

        public IEnumerable<ReservationInfo> Reservations { get; set; }

        public class ReservationInfo {

            public int Id { get; set; }

            public int ResourceId { get; set; }

            public string ResourceName { get; set; }

            public DateTime DateBegin { get; set; }

            public DateTime DateEnd { get; set; }

            public bool CanBeDeleted { get; set; }

        }

        public async Task OnGetAsync() {
            // Get resources accessible to user
            var resourcesQuery = this.dc.Resources.OrderBy(x => x.Name);
            if (!this.User.IsPrivilegedUser()) resourcesQuery = (IOrderedQueryable<Resource>)resourcesQuery.Where(x => x.Enabled);
            this.Resources = await resourcesQuery.ToListAsync();

            // Get reservations of this user
            var userId = int.Parse(this.userManager.GetUserId(this.User));
            var now = this.dateProvider.Now;
            var reservationsQuery = from r in this.dc.Reservations
                                    where r.UserId == userId && r.DateEnd >= this.dateProvider.Today
                                    orderby r.DateBegin
                                    select new ReservationInfo {
                                        DateBegin = r.DateBegin,
                                        DateEnd = r.DateEnd,
                                        Id = r.Id,
                                        ResourceId = r.ResourceId,
                                        ResourceName = r.Resource.Name,
                                        CanBeDeleted = r.DateEnd > this.dateProvider.Now
                                    };
            this.Reservations = await reservationsQuery.ToListAsync();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int reservationId) {
            var userId = int.Parse(this.userManager.GetUserId(this.User));
            var reservation = await this.dc.Reservations.SingleOrDefaultAsync(x => x.Id == reservationId && x.UserId == userId && x.DateEnd > this.dateProvider.Now);
            if (reservation == null) return this.NotFound();
            this.dc.Reservations.Remove(reservation);
            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("Index", null, "reservationdeleted");
        }

    }
}
