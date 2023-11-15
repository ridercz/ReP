using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Altairis.ReP.Web.Pages.Login;
public class ActivateModel(UserManager<ApplicationUser> userManager) : PageModel {
    
    private readonly UserManager<ApplicationUser> userManager = userManager;

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public string NewUserName { get; set; } = string.Empty;

    public class InputModel {

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }

    public async Task<IActionResult> OnGetAsync(int userId, string token) {
        // Get user
        var user = await this.userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.EmailConfirmed) return this.RedirectToPage("Index", null, "afail");
        this.NewUserName = user.UserName ?? throw new ImpossibleException();

        // Try to confirm e-mail address
        var result = await this.userManager.ConfirmEmailAsync(user, token);
        return this.IsIdentitySuccess(result) ? this.Page() : this.RedirectToPage("Index", null, "afail");
    }

    public async Task<IActionResult> OnPostAsync(int userId) {
        if (!this.ModelState.IsValid) return this.Page();

        // Get user
        var user = await this.userManager.FindByIdAsync(userId.ToString());
        if (user == null) return this.NotFound();
        this.NewUserName = user.UserName ?? throw new ImpossibleException();


        // Try to set password
        var result = await this.userManager.AddPasswordAsync(user, this.Input.Password);
        return this.IsIdentitySuccess(result) ? this.RedirectToPage("Index", null, "adone") : this.Page();
    }

}
