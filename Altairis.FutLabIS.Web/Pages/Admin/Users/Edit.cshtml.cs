using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.FutLabIS.Web.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.FutLabIS.Web.Pages.Admin.Users {
    public class EditModel : PageModel {
        private readonly UserManager<ApplicationUser> userManager;

        public EditModel(UserManager<ApplicationUser> userManager) {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, MaxLength(50)]
            public string UserName { get; set; }

            [Required, MaxLength(50), EmailAddress]
            public string Email { get; set; }

            public string Language { get; set; }

            [MaxLength(20), Phone]
            public string PhoneNumber { get; set; }

            public bool IsMaster { get; set; }

            public bool IsAdministrator { get; set; }

            public bool UserEnabled { get; set; }

        }

        public IEnumerable<SelectListItem> AllLanguages { get; } = new List<SelectListItem>() {
            new SelectListItem(UI.My_Settings_Index_Language_CS, "cs-CZ"),
            new SelectListItem(UI.My_Settings_Index_Language_EN, "en-US")
        };

        public async Task<IActionResult> OnGetAsync(int userId) {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null) return this.NotFound();

            this.Input = new InputModel {
                Email = user.Email,
                UserEnabled = user.Enabled,
                Language = user.Language,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                IsAdministrator = await this.userManager.IsInRoleAsync(user, ApplicationRole.Administrator),
                IsMaster = await this.userManager.IsInRoleAsync(user, ApplicationRole.Master)
            };

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(int userId) {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null) return this.NotFound();

            if (!this.ModelState.IsValid) return this.Page();

            user.Email = this.Input.Email;
            user.Enabled = this.Input.UserEnabled;
            user.PhoneNumber = this.Input.PhoneNumber;
            user.UserName = this.Input.UserName;
            user.Language = this.Input.Language;

            var result = await this.userManager.UpdateAsync(user);
            if (!this.IsIdentitySuccess(result)) return this.Page();

            Task<IdentityResult> SetUserMembership(ApplicationUser user, string role, bool status) {
                if (status) return this.userManager.AddToRoleAsync(user, role);
                else return this.userManager.RemoveFromRoleAsync(user, role);
            }

            await SetUserMembership(user, ApplicationRole.Administrator, this.Input.IsAdministrator);
            await SetUserMembership(user, ApplicationRole.Master, this.Input.IsMaster);

            return this.RedirectToPage("Index", null, "saved");

        }

        public async Task<IActionResult> OnPostDeleteAsync(int userId) {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null) return this.NotFound();

            await this.userManager.DeleteAsync(user);

            return this.RedirectToPage("Index", null, "deleted");
        }
    }
}
