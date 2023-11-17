using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.Login;

public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel {
    private readonly SignInManager<ApplicationUser> signInManager = signInManager;

    // Handlers

    public async Task<IActionResult> OnGetAsync() {
        await this.signInManager.SignOutAsync();
        return this.RedirectToPage("Index", null, "logout");
    }
}

