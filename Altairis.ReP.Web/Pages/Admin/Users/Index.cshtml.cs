using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ReP.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.ReP.Web.Pages.Admin.Users {
    public class IndexModel : PageModel {
        private readonly RepDbContext dc;

        public IndexModel(RepDbContext dc) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        }

        public class UserInfo {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Language { get; set; }
            public string PhoneNumber { get; set; }
            public bool Enabled { get; set; }

            public bool EmailConfirmed { get; set; }
        }


        public IEnumerable<UserInfo> Users { get; set; }

        public async Task OnGetAsync() {
            var q = from u in this.dc.Users
                    orderby u.UserName
                    select new UserInfo {
                        Id = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                        Language = u.Language,
                        PhoneNumber = u.PhoneNumber,
                        Enabled = u.Enabled,
                        EmailConfirmed = u.EmailConfirmed
                    };
            this.Users = await q.ToListAsync();
        }

    }
}
