using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Web.Pages.Admin.NewsMessages {
    public class IndexModel : PageModel {
        private readonly FutLabDbContext dc;

        public IndexModel(FutLabDbContext dc) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        }

        public IEnumerable<NewsMessageInfo> NewsMessages { get; set; }

        public class NewsMessageInfo {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string Title { get; set; }
        }

        public async Task OnGetAsync() {
            var q = from m in this.dc.NewsMessages
                    orderby m.Date descending
                    select new NewsMessageInfo {
                        Id = m.Id,
                        Date = m.Date,
                        Title = m.Title
                    };
            this.NewsMessages = await q.ToListAsync();
        }

    }
}
