using Altairis.ReP.Web.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.ReP.Web.Pages.Admin.Users;

public class EditModel(UserManager<ApplicationUser> userManager, IOptions<AppSettings> options) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Language { get; set; } = string.Empty;

        [MaxLength(20), Phone]
        public string? PhoneNumber { get; set; }

        public bool IsMaster { get; set; }

        public bool IsAdministrator { get; set; }

        public bool UserEnabled { get; set; }

        [Required, MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        public bool ShowInMemberDirectory { get; set; }

    }

    // Output model

    public IEnumerable<SelectListItem> AllLanguages => LanguageSwitchViewComponent.AvailableCultures.Select(c => new SelectListItem(c.NativeName, c.Name));

    // Handlers

    public async Task<IActionResult> OnGetAsync(int userId) {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null) return this.NotFound();

        this.Input = new InputModel {
            Email = user!.Email ?? throw new ImpossibleException(),
            UserEnabled = user.Enabled,
            Language = user.Language,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName ?? throw new ImpossibleException(),
            IsAdministrator = await userManager.IsInRoleAsync(user, ApplicationRole.Administrator),
            IsMaster = await userManager.IsInRoleAsync(user, ApplicationRole.Master),
            DisplayName = user.DisplayName,
            ShowInMemberDirectory = user.ShowInMemberDirectory
        };

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int userId) {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null) return this.NotFound();

        if (!this.ModelState.IsValid) return this.Page();

        user.Email = this.Input.Email.Trim();
        user.Enabled = this.Input.UserEnabled;
        user.PhoneNumber = this.Input.PhoneNumber?.Trim();
        user.UserName = this.Input.UserName.Trim();
        user.Language = this.Input.Language;
        user.DisplayName = this.Input.DisplayName.Trim();
        user.ShowInMemberDirectory = options.Value.Features.UseMemberDirectory && this.Input.ShowInMemberDirectory;

        var result = await userManager.UpdateAsync(user);
        if (!this.IsIdentitySuccess(result)) return this.Page();

        Task<IdentityResult> SetUserMembership(ApplicationUser user, string role, bool status) => status ? userManager.AddToRoleAsync(user, role) : userManager.RemoveFromRoleAsync(user, role);

        await SetUserMembership(user, ApplicationRole.Administrator, this.Input.IsAdministrator);
        await SetUserMembership(user, ApplicationRole.Master, this.Input.IsMaster);

        return this.RedirectToPage("Index", null, "saved");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int userId) {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null) return this.NotFound();

        await userManager.DeleteAsync(user);

        return this.RedirectToPage("Index", null, "deleted");
    }
}
