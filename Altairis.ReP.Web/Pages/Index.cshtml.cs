using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ReP.Web.Pages {
    public class IndexModel : PageModel {
        public IActionResult OnGet() => this.RedirectToPage("/My/Index");
    }
}
