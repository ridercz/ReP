using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.Login;

public class IndexModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }

    }

    // Handlers

    public async Task<IActionResult> OnGetAsync() => await userManager.Users.AnyAsync() ? this.Page() : this.RedirectToPage("/FirstRun");

    public async Task<IActionResult> OnPostAsync(string returnUrl = "/") {
        if (this.ModelState.IsValid) {
            var result = await signInManager.PasswordSignInAsync(
                this.Input.UserName.Trim(),
                this.Input.Password.Trim(),
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
