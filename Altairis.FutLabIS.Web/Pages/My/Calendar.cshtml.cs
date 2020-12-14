using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.Services.DateProvider;
using Altairis.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Web.Pages.My {
    public class CalendarModel : PageModel {
        private readonly FutLabDbContext dc;
        private readonly IDateProvider dateProvider;

        public CalendarModel(FutLabDbContext dc, IDateProvider dateProvider) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
            this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        }

        public IEnumerable<CalendarEvent> Reservations { get; set; }

        public IEnumerable<ResourceTag> Resources { get; set; }

        public class ResourceTag {
            public string Name { get; set; }
            public string ForegroundColor { get; set; }
            public string BackgroundColor { get; set; }

            public string GetStyle() => $"color:{this.ForegroundColor};background-color:{this.BackgroundColor};";
        }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public DateTime DatePrev { get; set; }

        public DateTime DateNext { get; set; }

        public async Task<IActionResult> OnGetAsync(int? year, int? month) {
            // Redirect to current month
            if (!year.HasValue || !month.HasValue) return this.RedirectToPage(new { this.dateProvider.Today.Year, this.dateProvider.Today.Month });

            // Get month name for display
            this.DateBegin = new DateTime(year.Value, month.Value, 1);
            this.DateEnd = this.DateBegin.AddMonths(1).AddDays(-1);
            this.DatePrev = this.DateBegin.AddMonths(-1);
            this.DateNext = this.DateBegin.AddMonths(+1);

            // Get all resources for tags
            this.Resources = await this.dc.Resources
                .OrderBy(x => x.Name)
                .Select(x => new ResourceTag {
                    Name = x.Name,
                    ForegroundColor = x.ForegroundColor,
                    BackgroundColor = x.BackgroundColor
                })
                .ToListAsync();

            // Get all reservations in this month
            var q = from r in this.dc.Reservations
                    where r.DateEnd >= this.DateBegin.AddDays(-6) && r.DateBegin < this.DateEnd.AddDays(6)
                    orderby r.DateBegin
                    select new CalendarEvent {
                        BackgroundColor = r.Resource.BackgroundColor,
                        CssClass = r.System ? "system" : string.Empty,
                        DateBegin = r.DateBegin,
                        DateEnd = r.DateEnd,
                        Description = r.Comment,
                        ForegroundColor = r.Resource.ForegroundColor,
                        IsFullDay = false,
                        Name = r.User.UserName
                    };
            this.Reservations = await q.ToListAsync();

            return this.Page();
        }

    }
}
