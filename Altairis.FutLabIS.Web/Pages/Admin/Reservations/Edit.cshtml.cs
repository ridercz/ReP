using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.FutLabIS.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Web.Pages.Admin.Reservations {
    public class EditModel : PageModel {
        private readonly FutLabDbContext dc;

        public EditModel(FutLabDbContext dc) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            public DateTime DateBegin { get; set; }

            public DateTime DateEnd { get; set; }

            public bool System { get; set; }

            public string Comment { get; set; }

        }

        public int ResourceId { get; set; }

        public string ResourceName { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }


        public async Task<Reservation> Init(int reservationId) {
            var r = await this.dc.Reservations.Include(x => x.Resource).Include(x => x.User).SingleOrDefaultAsync(x => x.Id == reservationId);
            if (r != null) {
                this.ResourceId = r.ResourceId;
                this.ResourceName = r.Resource.Name;
                this.UserId = r.UserId;
                this.UserName = r.User.UserName;
            }
            return r;
        }

        public async Task<IActionResult> OnGetAsync(int reservationId) {
            var r = await this.Init(reservationId);
            if (r == null) return this.NotFound();

            this.Input = new InputModel {
                Comment = r.Comment,
                DateBegin = r.DateBegin,
                DateEnd = r.DateEnd,
                System = r.System
            };

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(int reservationId) {
            var r = await this.Init(reservationId);
            if (r == null) return this.NotFound();
            if (!this.ModelState.IsValid) return this.Page();

            // Check reservation for conflicts
            var q = from cr in this.dc.Reservations
                    where cr.DateBegin < this.Input.DateEnd && cr.DateEnd > this.Input.DateBegin && cr.Id != r.Id
                    select new { cr.DateBegin, cr.User.UserName };
            foreach (var item in await q.ToListAsync()) {
                this.ModelState.AddModelError(string.Empty, string.Format(UI.My_Reservations_Err_Conflict, item.UserName, item.DateBegin));
            }
            if (!this.ModelState.IsValid) return this.Page();

            // Update reservation
            r.Comment = this.Input.Comment;
            r.DateBegin = this.Input.DateBegin;
            r.DateEnd = this.Input.DateEnd;
            r.System = this.Input.System;

            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("Index", null, "saved");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int reservationId) {
            var reservation = await this.dc.Reservations.FindAsync(reservationId);
            if (reservation == null) return this.NotFound();
            this.dc.Reservations.Remove(reservation);
            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("Index", null, "deleted");
        }

    }
}
