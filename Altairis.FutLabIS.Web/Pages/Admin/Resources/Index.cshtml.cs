using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Web.Pages.Admin.Resources {
    public class IndexModel : PageModel {
        private readonly FutLabDbContext dc;

        public IndexModel(FutLabDbContext dc) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        }

        public IEnumerable<ResourceInfo> Resources { get; set; }

        public class ResourceInfo {
            public string Description { get; set; }
            public string Name { get; set; }
            public int Id { get; set; }
        }

        public async Task OnGetAsync() {
            var q = from r in this.dc.Resources
                    orderby r.Name
                    select new ResourceInfo {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description
                    };
            this.Resources = await q.ToListAsync();
        }

    }
}
