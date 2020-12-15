using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.FutLabIS.Web.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages.Login {
    public class ResetPasswordModel : PageModel {
        private readonly UserManager<ApplicationUser> userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager) {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

        }

        public async Task<IActionResult> OnPostAsync(string userId, string token) {
            if (!this.ModelState.IsValid) return this.Page();

            // Try to find user by ID
            var user = await this.userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null) {
                this.ModelState.AddModelError(nameof(this.Input.Password), UI.Login_ForgotPassword_UserNotFound);
                return this.Page();
            }

            // Try to reset password
            var result = await this.userManager.ResetPasswordAsync(
                user,
                token,
                this.Input.Password);

            if (this.IsIdentitySuccess(result)) {
                // Set user e-mail address as confirmed
                user.EmailConfirmed = true;
                await this.userManager.UpdateAsync(user);

                // Redirect to confirmation page
                return this.RedirectToPage("Index", null, "reset");
            }

            return this.Page();
        }
    }
}
