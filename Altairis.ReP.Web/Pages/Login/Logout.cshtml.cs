using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.Login;

public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel {

    // Handlers

    public async Task<IActionResult> OnGetAsync() {
        await signInManager.SignOutAsync();
        return this.RedirectToPage("Index", null, "logout");
    }
}

