using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Web.Pages.Admin {
    public class ReservationsModel : PageModel {
        private readonly FutLabDbContext dc;

        public ReservationsModel(FutLabDbContext dc) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        }

        public class ReservationInfo {

            public int ReservationId { get; set; }

            public string UserName { get; set; }

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
                        UserName = r.User.UserName
                    };
            this.Reservations = await q.ToListAsync();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int reservationId) {
            var reservation = await this.dc.Reservations.FindAsync(reservationId);
            if (reservation == null) return this.NotFound();
            this.dc.Reservations.Remove(reservation);
            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("", null, "reservationdeleted");
        }
    }
}