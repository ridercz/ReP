using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.Login;
public class ActivateModel : PageModel {
    private readonly UserManager<ApplicationUser> userManager;

    public ActivateModel(UserManager<ApplicationUser> userManager) {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public string NewUserName { get; set; }

    public class InputModel {

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public async Task<IActionResult> OnGetAsync(int userId, string token) {
        // Get user
        var user = await this.userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.EmailConfirmed) return this.RedirectToPage("Index", null, "afail");
        this.NewUserName = user.UserName;

        // Try to confirm e-mail address
        var result = await this.userManager.ConfirmEmailAsync(user, token);
        if (!this.IsIdentitySuccess(result)) return this.RedirectToPage("Index", null, "afail");
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int userId) {
        if (!this.ModelState.IsValid) return this.Page();

        // Get user
        var user = await this.userManager.FindByIdAsync(userId.ToString());
        if (user == null) return this.NotFound();
        this.NewUserName = user.UserName;


        // Try to set password
        var result = await this.userManager.AddPasswordAsync(user, this.Input.Password);
        if (!this.IsIdentitySuccess(result)) return this.Page();

        return this.RedirectToPage("Index", null, "adone");
    }

}
