using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages.My.Settings {
    public class PasswordModel : PageModel {
        private readonly UserManager<ApplicationUser> userManager;

        public PasswordModel(UserManager<ApplicationUser> userManager) {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, DataType(DataType.Password)]
            public string CurrentPassword { get; set; }

            [Required, DataType(DataType.Password)]
            public string NewPassword { get; set; }

        }

        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) return this.Page();
            var me = await this.userManager.GetUserAsync(this.User);
            var result = await this.userManager.ChangePasswordAsync(me, this.Input.CurrentPassword, this.Input.NewPassword);
            return this.IsIdentitySuccess(result) ? this.RedirectToPage("Index", null, "changepassword") : (IActionResult)this.Page();
        }

    }
}
