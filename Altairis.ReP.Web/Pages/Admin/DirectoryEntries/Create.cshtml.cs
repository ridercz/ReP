namespace Altairis.ReP.Web.Pages.Admin.DirectoryEntries;

public class CreateModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc ?? throw new ArgumentNullException(nameof(dc));

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

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        this.dc.DirectoryEntries.Add(new DirectoryEntry {
            DisplayName = this.Input.DisplayName,
            Email = this.Input.Email,
            PhoneNumber = this.Input.PhoneNumber,
        });
        await this.dc.SaveChangesAsync();

        return this.RedirectToPage("Index", null, "created");
    }
}
