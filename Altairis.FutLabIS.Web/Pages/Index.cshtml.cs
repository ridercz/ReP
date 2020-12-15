using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages {
    public class IndexModel : PageModel {
        public IActionResult OnGet() => this.RedirectToPage("/My/Index");
    }
}
