using System.Globalization;
using Altairis.ReP.Web.Components;
using Altairis.Services.Mailing.Templating;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.ReP.Web.Pages.Admin.Users;
public class CreateModel(UserManager<ApplicationUser> userManager, ITemplatedMailerService mailerService, IOptions<AppSettings> options) : PageModel {
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly ITemplatedMailerService mailer = mailerService;
    private readonly IOptions<AppSettings> options = options;

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        public bool ShowInMemberDirectory { get; set; }

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20), Phone]
        public string? PhoneNumber { get; set; }

        public bool IsMaster { get; set; }

        public bool IsAdministrator { get; set; }

        public string Language { get; set; } = "cs-CZ";

    }

    public IEnumerable<SelectListItem> AllLanguages => LanguageSwitchViewComponent.AvailableCultures.Select(c => new SelectListItem(c.NativeName, c.Name));

    public void OnGet() => this.Input.ShowInMemberDirectory = this.options.Value.Features.UseMemberDirectory;

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        // Create new user
        var newUser = new ApplicationUser {
            UserName = this.Input.UserName,
            Email = this.Input.Email,
            PhoneNumber = this.Input.PhoneNumber,
            Language = this.Input.Language,
            DisplayName = this.Input.DisplayName,
            ShowInMemberDirectory = this.options.Value.Features.UseMemberDirectory && this.Input.ShowInMemberDirectory,
            ResourceAuthorizationKey = ApplicationUser.CreateResourceAuthorizationKey()
        };
        var result = await this.userManager.CreateAsync(newUser);
        if (!this.IsIdentitySuccess(result)) return this.Page();

        // Assign roles
        if (this.Input.IsMaster) await this.userManager.AddToRoleAsync(newUser, ApplicationRole.Master);
        if (this.Input.IsAdministrator) await this.userManager.AddToRoleAsync(newUser, ApplicationRole.Administrator);

        // Get e-mail confirmation URL
        var token = await this.userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var activationUrl = this.Url.Page("/Login/Activate", pageHandler: null, values: new { UserId = newUser.Id, Token = token }, protocol: this.Request.Scheme);

        // Send welcome mail
        var culture = new CultureInfo(this.Input.Language);
        var msg = new TemplatedMailMessageDto("Activation", newUser.Email);
        await this.mailer.SendMessageAsync(msg, new {
            userName = newUser.UserName,
            url = activationUrl
        }, culture, culture);

        return this.RedirectToPage("Index", null, "created");
    }

}
