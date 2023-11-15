using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages;
public class FirstRunModel(UserManager<ApplicationUser> userManager) : PageModel {
    private readonly UserManager<ApplicationUser> userManager = userManager;

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }

    public async Task<IActionResult> OnGetAsync() {
        if (await this.userManager.Users.AnyAsync()) return this.NotFound();
        this.Input.Password = SecurityHelper.GenerateRandomPassword();
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync() {
        if (await this.userManager.Users.AnyAsync()) return this.NotFound();
        if (!this.ModelState.IsValid) return this.Page();

        // Create user
        var user = new ApplicationUser {
            UserName = this.Input.UserName,
            DisplayName = this.Input.DisplayName,
            Email = this.Input.Email,
            EmailConfirmed = true,
            Language = CultureInfo.CurrentUICulture.Name,
            Enabled = true,
            ResourceAuthorizationKey = ApplicationUser.CreateResourceAuthorizationKey()
        };
        if (!this.IsIdentitySuccess(await this.userManager.CreateAsync(user, this.Input.Password))) return this.Page();

        // Assign Administrator role
        if (!this.IsIdentitySuccess(await this.userManager.AddToRoleAsync(user, ApplicationRole.Administrator))) return this.Page();

        // Redirect to home page
        return this.RedirectToPage("My/Index");
    }

}
