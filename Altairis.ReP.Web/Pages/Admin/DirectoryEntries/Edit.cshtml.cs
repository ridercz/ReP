namespace Altairis.ReP.Web.Pages.Admin.DirectoryEntries;

public class EditModel : PageModel {
    private readonly RepDbContext dc;

    public EditModel(RepDbContext dc) {
        this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(100)]
        public string DisplayName { get; set; }

        [MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [MaxLength(50), Phone]
        public string PhoneNumber { get; set; }

    }

    public async Task<IActionResult> OnGetAsync(int directoryEntryId) {
        var de = await this.dc.DirectoryEntries.FindAsync(directoryEntryId);
        if (de == null) return this.NotFound();

        this.Input = new InputModel {
            DisplayName = de.DisplayName,
            Email = de.Email,
            PhoneNumber = de.PhoneNumber

        };
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int directoryEntryId) {
        var de = await this.dc.DirectoryEntries.FindAsync(directoryEntryId);
        if (de == null) return this.NotFound();

        if (!this.ModelState.IsValid) return this.Page();

        de.DisplayName = this.Input.DisplayName;
        de.PhoneNumber = this.Input.PhoneNumber;
        de.Email = this.Input.Email;

        await this.dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "saved");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int directoryEntryId) {
        var de = await this.dc.DirectoryEntries.FindAsync(directoryEntryId);
        if (de == null) return this.NotFound();

        this.dc.DirectoryEntries.Remove(de);

        await this.dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "deleted");
    }

}
