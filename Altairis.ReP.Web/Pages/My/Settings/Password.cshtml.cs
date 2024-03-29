using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My.Settings;

public class PasswordModel(UserManager<ApplicationUser> userManager) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

    }

    // Handlers

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();
        var me = await userManager.GetUserAsync(this.User) ?? throw new ImpossibleException();
        var result = await userManager.ChangePasswordAsync(me, this.Input.CurrentPassword, this.Input.NewPassword);
        return this.IsIdentitySuccess(result) ? this.RedirectToPage("Index", null, "changepassword") : this.Page();
    }

}
