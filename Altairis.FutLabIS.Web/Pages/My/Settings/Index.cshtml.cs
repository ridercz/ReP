using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.FutLabIS.Web.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.FutLabIS.Web.Pages.My.Settings {
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

        }

        public IEnumerable<SelectListItem> AllLanguages { get; } = new List<SelectListItem>() {
            new SelectListItem(UI.My_Settings_Index_Language_CS, "cs"),
            new SelectListItem(UI.My_Settings_Index_Language_EN, "en")
        };

        public ApplicationUser Me { get; set; }

        public async Task OnGetAsync() {
            this.Me = await this.userManager.GetUserAsync(this.User);
            this.Input.Language = this.Me.Language;
            this.Input.PhoneNumber = this.Me.PhoneNumber;
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) return this.Page();
            this.Me = await this.userManager.GetUserAsync(this.User);

            this.Me.Language = this.Input.Language;
            this.Me.PhoneNumber = this.Input.PhoneNumber;
            await this.userManager.UpdateAsync(this.Me);

            return this.RedirectToPage("Index", null, "saved");
        }

    }
}
