using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using Altairis.ReP.Data;
using Altairis.ReP.Web.Resources;
using Altairis.Services.Mailing.Templating;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.ReP.Web.Pages.Admin.Users {
    public class CreateModel : PageModel {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITemplatedMailerService mailerService;

        public CreateModel(UserManager<ApplicationUser> userManager, ITemplatedMailerService mailerService) {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mailerService = mailerService ?? throw new ArgumentNullException(nameof(mailerService));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, MaxLength(50)]
            public string UserName { get; set; }

            [Required, MaxLength(50), EmailAddress]
            public string Email { get; set; }

            [MaxLength(20), Phone]
            public string PhoneNumber { get; set; }

            public bool IsMaster { get; set; }

            public bool IsAdministrator { get; set; }

            public string Language { get; set; } = "cs-CZ";

        }

        public IEnumerable<SelectListItem> AllLanguages { get; } = new List<SelectListItem>() {
            new SelectListItem(UI.My_Settings_Index_Language_CS, "cs-CZ"),
            new SelectListItem(UI.My_Settings_Index_Language_EN, "en-US")
        };

        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) return this.Page();

            // Create new user
            var newUser = new ApplicationUser {
                UserName = this.Input.UserName,
                Email = this.Input.Email,
                PhoneNumber = this.Input.PhoneNumber,
                Language = this.Input.Language
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
            await this.mailerService.SendMessageAsync(msg, new {
                userName = newUser.UserName,
                url = activationUrl
            }, culture, culture);

            return this.RedirectToPage("Index", null, "created");
        }



    }
}
