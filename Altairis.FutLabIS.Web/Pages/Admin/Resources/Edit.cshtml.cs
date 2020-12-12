using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages.Admin.Resources {
    public class EditModel : PageModel {
        private readonly FutLabDbContext dc;

        public EditModel(FutLabDbContext dc) {
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

        }

        public async Task<IActionResult> OnGetAsync(int resourceId) {
            var resource = await this.dc.Resources.FindAsync(resourceId);
            if (resource == null) return this.NotFound();

            this.Input = new InputModel {
                Description = resource.Description,
                Enabled = resource.Enabled,
                MaximumReservationTime = resource.MaximumReservationTime,
                Name = resource.Name
            };
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(int resourceId) {
            var resource = await this.dc.Resources.FindAsync(resourceId);
            if (resource == null) return this.NotFound();

            if (!this.ModelState.IsValid) return this.Page();

            resource.Description = this.Input.Description;
            resource.Enabled = this.Input.Enabled;
            resource.MaximumReservationTime = this.Input.MaximumReservationTime;
            resource.Name = this.Input.Name;

            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("Index", null, "saved");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int resourceId) {
            var resource = await this.dc.Resources.FindAsync(resourceId);
            if (resource == null) return this.NotFound();

            this.dc.Resources.Remove(resource);

            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("Index", null, "deleted");
        }
    }
}
