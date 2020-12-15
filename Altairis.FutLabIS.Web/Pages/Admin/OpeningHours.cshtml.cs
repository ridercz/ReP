using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.FutLabIS.Web.Services;
using Altairis.ValidationToolkit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Web.Pages.Admin {
    public class OpeningHoursModel : PageModel {
        private readonly FutLabDbContext dc;
        private readonly OpeningHoursProvider hoursProvider;

        public OpeningHoursModel(FutLabDbContext dc, OpeningHoursProvider hoursProvider) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
            this.hoursProvider = hoursProvider ?? throw new ArgumentNullException(nameof(hoursProvider));
        }

        public IEnumerable<OpeningHoursInfo> StandardOpeningHours => this.hoursProvider.GetStandardOpeningHours();

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public IEnumerable<OpeningHoursChange> OpeningHoursChanges { get; set; }

        public class InputModel {

            [DataType(DataType.Date)]
            public DateTime Date { get; set; } = DateTime.Today.AddDays(1);

            [DataType(DataType.Time), Range(typeof(TimeSpan), "00:00:00", "23:59:59")]
            public TimeSpan OpeningTime { get; set; } = TimeSpan.Zero;

            [DataType(DataType.Time), Range(typeof(TimeSpan), "00:00:00", "23:59:59"), GreaterThan(nameof(OpeningTime), AllowEqual = true)]
            public TimeSpan ClosingTime { get; set; } = TimeSpan.Zero;

        }

        public async Task OnGetAsync() {
            this.OpeningHoursChanges = await this.dc.OpeningHoursChanges.OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) return this.Page();

            var item = await this.dc.OpeningHoursChanges.SingleOrDefaultAsync(x => x.Date == this.Input.Date);
            if (item == null) {
                this.dc.OpeningHoursChanges.Add(new OpeningHoursChange {
                    Date = this.Input.Date,
                    OpeningTime = this.Input.OpeningTime,
                    ClosingTime = this.Input.ClosingTime
                });
            } else {
                item.OpeningTime = this.Input.OpeningTime;
                item.ClosingTime = this.Input.ClosingTime;
            }

            await this.dc.SaveChangesAsync();
            return this.RedirectToPage(string.Empty, null, "created");
        }

        public async Task<IActionResult> OnGetDeleteAsync(int ohchId) {
            var item = await this.dc.OpeningHoursChanges.FindAsync(ohchId);
            if (item == null) return this.NotFound();
            this.dc.OpeningHoursChanges.Remove(item);
            await this.dc.SaveChangesAsync();
            return this.RedirectToPage(string.Empty, null, "deleted");
        }

    }
}
