using Altairis.Services.Mailing.Templating;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.Login;
public class ForgotPasswordModel : PageModel {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ITemplatedMailerService mailerService;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, ITemplatedMailerService mailerService) {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.mailerService = mailerService ?? throw new ArgumentNullException(nameof(mailerService));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required]
        public string UserName { get; set; }

    }

    public async Task<IActionResult> OnPostAsync(string locale) {
        if (!this.ModelState.IsValid) return this.Page();

        // Try to find user by name
        var user = await this.userManager.FindByNameAsync(this.Input.UserName);
        if (user == null) {
            this.ModelState.AddModelError(nameof(this.Input.UserName), UI.Login_ForgotPassword_UserNotFound);
            return this.Page();
        }

        // Get password reset token
        var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

        // Get password reset URL
        var passwordResetUrl = this.Url.Page("/Login/ResetPassword",
            pageHandler: null,
            values: new { userId = user.Id, token = token },
            protocol: this.Request.Scheme);

        // Send password reset mail
        var msg = new TemplatedMailMessageDto("PasswordReset", user.Email);
        await this.mailerService.SendMessageAsync(msg, new {
            userName = user.UserName,
            url = passwordResetUrl
        });

        return this.RedirectToPage("Index", null, "sent");
    }

}
