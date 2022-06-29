using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My.Settings;
public class PasswordModel : PageModel {
    private readonly UserManager<ApplicationUser> userManager;

    public PasswordModel(UserManager<ApplicationUser> userManager) {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }

    }

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();
        var me = await this.userManager.GetUserAsync(this.User);
        var result = await this.userManager.ChangePasswordAsync(me, this.Input.CurrentPassword, this.Input.NewPassword);
        return this.IsIdentitySuccess(result) ? this.RedirectToPage("Index", null, "changepassword") : (IActionResult)this.Page();
    }

}
