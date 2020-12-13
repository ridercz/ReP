using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.FutLabIS.Web.Resources;
using Altairis.Services.DateProvider;
using Altairis.ValidationToolkit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Web.Pages.My {
    public class ReservationsModel : PageModel {
        private readonly FutLabDbContext dc;
        private readonly IDateProvider dateProvider;
        private readonly UserManager<ApplicationUser> userManager;

        public ReservationsModel(FutLabDbContext dc, IDateProvider dateProvider, UserManager<ApplicationUser> userManager) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
            this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty]
        public InputModel NormalReservation { get; set; } = new InputModel();

        public class InputModel {

            public int ResourceId { get; set; }

            [DataType(DataType.DateTime), DateOffset(0, 1, CompareTime = true)]
            public DateTime DateBegin { get; set; }

            [DataType(DataType.DateTime), GreaterThan(nameof(DateBegin))]
            public DateTime DateEnd { get; set; }

            public bool System { get; set; }

            public string Comment { get; set; }

        }

        public Resource Resource { get; set; }

        private async Task<bool> Init(int resourceId) {
            this.Resource = await this.dc.Resources.SingleOrDefaultAsync(x => x.Id == resourceId);
            return this.Resource != null;
        }

        public async Task<IActionResult> OnGetAsync(int resourceId) {
            if (!await this.Init(resourceId)) return this.NotFound();

            var dt = this.dateProvider.Now.AddDays(1);
            this.NormalReservation.DateBegin = dt.AddMinutes(-dt.Minute);
            this.NormalReservation.DateEnd = this.NormalReservation.DateBegin.AddHours(1);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(int resourceId) {
            if (!(this.ModelState.IsValid && await this.Init(resourceId) && this.Resource.Enabled)) return this.NotFound();

            // Check reservation time length
            var resLength = this.NormalReservation.DateEnd.Subtract(this.NormalReservation.DateBegin).TotalMinutes;
            if (resLength > this.Resource.MaximumReservationTime) {
                this.ModelState.AddModelError(string.Empty, string.Format(UI.My_Reservations_Err_Maxlength, this.Resource.MaximumReservationTime));
                return this.Page();
            }

            // Create reservation
            var newReservation = new Reservation {
                DateBegin = this.NormalReservation.DateBegin,
                DateEnd = this.NormalReservation.DateEnd,
                UserId = int.Parse(this.userManager.GetUserId(this.User)),
                ResourceId = resourceId,
                System = false
            };
            this.dc.Reservations.Add(newReservation);
            await this.dc.SaveChangesAsync();

            // TODO: Check against lab opening times

            // TODO: Check against other reservations

            return this.RedirectToPage("Reservations", string.Empty, new { resourceId }, "created");
        }

    }

}
