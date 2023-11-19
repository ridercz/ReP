namespace Altairis.ReP.Web.Pages.Admin.DirectoryEntries;

public class EditModel(RepDbContext dc) : PageModel {

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

    public async Task<IActionResult> OnGetAsync(int directoryEntryId) {
        var de = await dc.DirectoryEntries.FindAsync(directoryEntryId);
        if (de == null) return this.NotFound();

        this.Input = new InputModel {
            DisplayName = de.DisplayName,
            Email = de.Email,
            PhoneNumber = de.PhoneNumber

        };
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int directoryEntryId) {
        var de = await dc.DirectoryEntries.FindAsync(directoryEntryId);
        if (de == null) return this.NotFound();

        if (!this.ModelState.IsValid) return this.Page();

        de.DisplayName = this.Input.DisplayName;
        de.PhoneNumber = this.Input.PhoneNumber;
        de.Email = this.Input.Email;

        await dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "saved");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int directoryEntryId) {
        var de = await dc.DirectoryEntries.FindAsync(directoryEntryId);
        if (de == null) return this.NotFound();

        dc.DirectoryEntries.Remove(de);

        await dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "deleted");
    }

}
