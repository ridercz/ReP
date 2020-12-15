using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.ValidationToolkit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages.Admin.Resources {
    public class CreateModel : PageModel {
        private readonly FutLabDbContext dc;

        public CreateModel(FutLabDbContext dc) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, MaxLength(50)]
            public string Name { get; set; }

            public string Description { get; set; }

            [Required, Range(0, 1440)]
            public int MaximumReservationTime { get; set; }

            public bool Enabled { get; set; } = true;

            [Required, Color]
            public string ForegroundColor { get; set; } = "#000000";

            [Required, Color]
            public string BackgroundColor { get; set; } = "#ffffff";

        }

        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) return this.Page();

            var newResource = new Resource {
                Description = this.Input.Description,
                Enabled = this.Input.Enabled,
                MaximumReservationTime = this.Input.MaximumReservationTime,
                Name = this.Input.Name,
                ForegroundColor = this.Input.ForegroundColor,
                BackgroundColor = this.Input.BackgroundColor
            };
            this.dc.Resources.Add(newResource);
            await this.dc.SaveChangesAsync();

            return this.RedirectToPage("Index", null, "created");
        }

    }
}
