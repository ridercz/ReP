using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.Login;

public class ActivateModel(UserManager<ApplicationUser> userManager) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();


    public class InputModel {

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }

    // Output model

    public string NewUserName { get; set; } = string.Empty;

    // Handlers

    public async Task<IActionResult> OnGetAsync(int userId, string token) {
        // Get user
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.EmailConfirmed) return this.RedirectToPage("Index", null, "afail");
        this.NewUserName = user.UserName ?? throw new ImpossibleException();

        // Try to confirm e-mail address
        var result = await userManager.ConfirmEmailAsync(user, token);
        return this.IsIdentitySuccess(result) ? this.Page() : this.RedirectToPage("Index", null, "afail");
    }

    public async Task<IActionResult> OnPostAsync(int userId) {
        if (!this.ModelState.IsValid) return this.Page();

        // Get user
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null) return this.NotFound();
        this.NewUserName = user.UserName ?? throw new ImpossibleException();


        // Try to set password
        var result = await userManager.AddPasswordAsync(user, this.Input.Password);
        return this.IsIdentitySuccess(result) ? this.RedirectToPage("Index", null, "adone") : this.Page();
    }

}
