using Altairis.ReP.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace Altairis.ReP.Web.Pages;
public class FirstRunModel : PageModel
{
    private readonly IUserService _service;
    private readonly UserManager<ApplicationUser> userManager;

    public FirstRunModel(IUserService service, UserManager<ApplicationUser> userManager)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(100)]
        public string DisplayName { get; set; }

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public async Task<IActionResult> OnGet(CancellationToken token)
    {
        if (await _service.IsThereAnyUserAsync(token)) return this.NotFound();
        this.Input.Password = SecurityHelper.GenerateRandomPassword();
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (await _service.IsThereAnyUserAsync(token)) return this.NotFound();
        if (!this.ModelState.IsValid) return this.Page();

        // Create user
        var user = new ApplicationUser
        {
            UserName = this.Input.UserName,
            DisplayName = this.Input.DisplayName,
            Email = this.Input.Email,
            EmailConfirmed = true,
            Language = CultureInfo.CurrentUICulture.Name,
            Enabled = true
        };
        if (!this.IsIdentitySuccess(await this.userManager.CreateAsync(user, this.Input.Password))) return this.Page();

        // Assign Administrator role
        if (!this.IsIdentitySuccess(await this.userManager.AddToRoleAsync(user, ApplicationRole.Administrator))) return this.Page();

        // Redirect to home page
        return this.RedirectToPage("Index");
    }

}
