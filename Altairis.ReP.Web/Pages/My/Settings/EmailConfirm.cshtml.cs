using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My.Settings;
public class EmailConfirmModel : PageModel {
    private readonly UserManager<ApplicationUser> userManager;

    public EmailConfirmModel(UserManager<ApplicationUser> userManager) {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IActionResult> OnGetAsync(string newEmail, string token) {
        var me = await this.userManager.GetUserAsync(this.User);
        if (me.Email.Equals(newEmail, StringComparison.OrdinalIgnoreCase)) return this.RedirectToPage("Index", null, "changeemaildone");

        var result = await this.userManager.ChangeEmailAsync(me, newEmail, token);
        return result.Succeeded ? this.RedirectToPage("Index", null, "changeemaildone") : (IActionResult)this.Page();
    }
}
