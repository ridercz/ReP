using Altairis.ReP.Web.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.ReP.Web.Pages.My.Settings;
public class IndexModel : PageModel {
    private readonly UserManager<ApplicationUser> userManager;

    public IndexModel(UserManager<ApplicationUser> userManager) {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        public string Language { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public bool SendNotifications { get; set; }

        public bool SendNews { get; set; }

        [Required, MaxLength(100)]
        public string DisplayName { get; set; }

        public bool ShowInMemberDirectory { get; set; }

    }

    public IEnumerable<SelectListItem> AllLanguages => LanguageSwitchViewComponent.GetAvailableCultures().Select(c => new SelectListItem(c.NativeName, c.Name));

    public ApplicationUser Me { get; set; }

    public async Task OnGetAsync() {
        this.Me = await this.userManager.GetUserAsync(this.User);
        this.Input.Language = this.Me.Language;
        this.Input.PhoneNumber = this.Me.PhoneNumber;
        this.Input.SendNotifications = this.Me.SendNotifications;
        this.Input.SendNews = this.Me.SendNews;
        this.Input.DisplayName = this.Me.DisplayName;
        this.Input.ShowInMemberDirectory = this.Me.ShowInMemberDirectory;
    }

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();
        this.Me = await this.userManager.GetUserAsync(this.User);

        this.Me.Language = this.Input.Language;
        this.Me.PhoneNumber = this.Input.PhoneNumber;
        this.Me.SendNews = this.Input.SendNews;
        this.Me.SendNotifications = this.Input.SendNotifications;
        this.Me.DisplayName = this.Input.DisplayName;
        this.Me.ShowInMemberDirectory = this.Input.ShowInMemberDirectory;
        await this.userManager.UpdateAsync(this.Me);

        return this.RedirectToPage("Index", null, "saved");
    }

    public async Task<IActionResult> OnPostResetRakAsync() {
        var me = await this.userManager.GetUserAsync(this.User);
        me.ResourceAuthorizationKey = ApplicationUser.CreateResourceAuthorizationKey();
        await this.userManager.UpdateAsync(me);
        return this.RedirectToPage("Index", null, "resetrakdone");
    }

}
