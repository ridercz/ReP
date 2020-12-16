using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.Services.DateProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages.Admin.NewsMessages {
    public class CreateModel : PageModel {
        private readonly FutLabDbContext dc;
        private readonly IDateProvider dateProvider;

        public CreateModel(FutLabDbContext dc, IDateProvider dateProvider) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
            this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, MaxLength(100)]
            public string Title { get; set; }

            [Required, DataType(DataType.MultilineText)]
            public string Text { get; set; }

        }

        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) return this.Page();

            var newMessage = new NewsMessage {
                Date = this.dateProvider.Now,
                Text = this.Input.Text,
                Title = this.Input.Title
            };
            this.dc.NewsMessages.Add(newMessage);
            await this.dc.SaveChangesAsync();

            return this.RedirectToPage("Index", null, "created");
        }

    }
}
