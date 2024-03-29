namespace Altairis.ReP.Web.Pages.Admin.DirectoryEntries;

public class CreateModel(RepDbContext dc) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        [MaxLength(100), EmailAddress]
        public string? Email { get; set; }

        [MaxLength(50), Phone]
        public string? PhoneNumber { get; set; }

    }

    // Handlers

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        dc.DirectoryEntries.Add(new DirectoryEntry {
            DisplayName = this.Input.DisplayName,
            Email = this.Input.Email,
            PhoneNumber = this.Input.PhoneNumber,
        });
        await dc.SaveChangesAsync();

        return this.RedirectToPage("Index", null, "created");
    }
}
