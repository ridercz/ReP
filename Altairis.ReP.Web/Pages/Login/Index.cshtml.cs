using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ReP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ReP.Web.Pages.Login {
    public class IndexModel : PageModel {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public IndexModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) {
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, MaxLength(50)]
            public string UserName { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }

        }

        public IActionResult OnGet() => this.userManager.Users.Any() ? this.Page() : (IActionResult)this.RedirectToPage("/FirstRun");

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/") {
            if (this.ModelState.IsValid) {
                var result = await this.signInManager.PasswordSignInAsync(
                    this.Input.UserName,
                    this.Input.Password,
                    this.Input.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded) {
                    return this.LocalRedirect(returnUrl);
                } else {
                    this.ModelState.AddModelError(string.Empty, Resources.UI.Login_Index_LoginFailed);
                }
            }
            return this.Page();
        }

    }
}
