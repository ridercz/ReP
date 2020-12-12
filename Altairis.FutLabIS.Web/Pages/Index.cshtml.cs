using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages {
    public class IndexModel : PageModel {
        public IActionResult OnGet() {
            return this.RedirectToPage("/My/Index");
        }
    }
}
