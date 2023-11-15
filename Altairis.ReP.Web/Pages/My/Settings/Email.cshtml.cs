using Altairis.Services.Mailing.Templating;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My.Settings;
public class EmailModel(UserManager<ApplicationUser> userManager, ITemplatedMailerService mailerService) : PageModel {
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly ITemplatedMailerService mailer = mailerService;

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;


        [Required, DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

    }

    public async Task OnGetAsync() {
        var me = await this.userManager.GetUserAsync(this.User) ?? throw new ImpossibleException();
        this.Input.Email = me.Email ?? throw new ImpossibleException();
    }

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        // Check if the address is really changed
        var me = await this.userManager.GetUserAsync(this.User) ?? throw new ImpossibleException();
        if (this.Input.Email.Equals(me.Email, StringComparison.OrdinalIgnoreCase)) return this.RedirectToPage("Index");

        // Check password
        var passwordCorrect = await this.userManager.CheckPasswordAsync(me, this.Input.CurrentPassword);
        if (!passwordCorrect) {
            this.ModelState.AddModelError(nameof(this.Input.CurrentPassword), UI.My_Settings_Email_InvalidPassword);
            return this.Page();
        }

        // Get email change token
        var token = await this.userManager.GenerateChangeEmailTokenAsync(me, this.Input.Email);

        // Get email change confirmation URL
        var url = this.Url.Page("/My/Settings/EmailConfirm",
            pageHandler: null,
            values: new {
                newEmail = this.Input.Email,
                token = token
            },
            protocol: this.Request.Scheme);

        // Send message
        var msg = new TemplatedMailMessageDto("EmailConfirm", this.Input.Email);
        await this.mailer.SendMessageAsync(msg, new {
            userName = me.UserName,
            url = url
        });

        return this.RedirectToPage("Index", null, "changeemail");
    }
}
