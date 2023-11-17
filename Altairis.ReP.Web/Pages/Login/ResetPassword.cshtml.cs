using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.Login;

public class ResetPasswordModel(UserManager<ApplicationUser> userManager) : PageModel {
    private readonly UserManager<ApplicationUser> userManager = userManager;

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }

    // Handlers

    public async Task<IActionResult> OnPostAsync(string userId, string token) {
        if (!this.ModelState.IsValid) return this.Page();

        // Try to find user by ID
        var user = await this.userManager.FindByIdAsync(userId);
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
