using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ReP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ReP.Web.Pages.Admin.NewsMessages {
    public class EditModel : PageModel {
        private readonly RepDbContext dc;

        public EditModel(RepDbContext dc) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, MaxLength(100)]
            public string Title { get; set; }

            [Required, DataType(DataType.MultilineText)]
            public string Text { get; set; }

        }

        public async Task<IActionResult> OnGetAsync(int newsMessageId) {
            var m = await this.dc.NewsMessages.FindAsync(newsMessageId);
            if (m == null) return this.NotFound();

            this.Input = new InputModel {
                Title = m.Title,
                Text = m.Text
            };
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(int newsMessageId) {
            var m = await this.dc.NewsMessages.FindAsync(newsMessageId);
            if (m == null) return this.NotFound();

            if (!this.ModelState.IsValid) return this.Page();

            m.Title = this.Input.Title;
            m.Text = this.Input.Text;

            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("Index", null, "saved");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int newsMessageId) {
            var m = await this.dc.NewsMessages.FindAsync(newsMessageId);
            if (m == null) return this.NotFound();

            this.dc.NewsMessages.Remove(m);

            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("Index", null, "deleted");
        }
    }
}
