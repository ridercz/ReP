using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages.Login {
    public class LogoutModel : PageModel {
        private readonly SignInManager<ApplicationUser> signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager) {
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<IActionResult> OnGetAsync(string done = null) {
            if (string.IsNullOrEmpty(done)) {
                await this.signInManager.SignOutAsync();
                return this.RedirectToPage(new { done = "done" });
            }
            return this.Page();
        }
    }
}
