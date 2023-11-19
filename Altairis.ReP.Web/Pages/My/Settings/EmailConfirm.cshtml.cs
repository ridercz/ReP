using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My.Settings;

public class EmailConfirmModel(UserManager<ApplicationUser> userManager) : PageModel {

    // Handlers

    public async Task<IActionResult> OnGetAsync(string newEmail, string token) {
        var me = await userManager.GetUserAsync(this.User) ?? throw new ImpossibleException();
        if (newEmail.Equals(me.Email, StringComparison.OrdinalIgnoreCase)) return this.RedirectToPage("Index", null, "changeemaildone");

        var result = await userManager.ChangeEmailAsync(me, newEmail, token);
        return result.Succeeded ? this.RedirectToPage("Index", null, "changeemaildone") : this.Page();
    }
}
