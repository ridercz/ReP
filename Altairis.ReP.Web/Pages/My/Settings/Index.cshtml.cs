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

    }

    public IEnumerable<SelectListItem> AllLanguages { get; } = new List<SelectListItem>() {
        new SelectListItem(UI.My_Settings_Index_Language_CS, "cs-CZ"),
        new SelectListItem(UI.My_Settings_Index_Language_EN, "en-US")
    };

    public ApplicationUser Me { get; set; }

    public async Task OnGetAsync() {
        this.Me = await this.userManager.GetUserAsync(this.User);
        this.Input.Language = this.Me.Language;
        this.Input.PhoneNumber = this.Me.PhoneNumber;
        this.Input.SendNotifications = this.Me.SendNotifications;
        this.Input.SendNews = this.Me.SendNews;
    }

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();
        this.Me = await this.userManager.GetUserAsync(this.User);

        this.Me.Language = this.Input.Language;
        this.Me.PhoneNumber = this.Input.PhoneNumber;
        this.Me.SendNews = this.Input.SendNews;
        this.Me.SendNotifications = this.Input.SendNotifications;
        await this.userManager.UpdateAsync(this.Me);

        return this.RedirectToPage("Index", null, "saved");
    }

}
