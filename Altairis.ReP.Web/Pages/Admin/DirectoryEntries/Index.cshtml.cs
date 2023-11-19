namespace Altairis.ReP.Web.Pages.Admin.DirectoryEntries;

public class IndexModel(RepDbContext dc) : PageModel {
    
    // Output model

    public IEnumerable<DirectoryEntry> DirectoryEntries { get; set; } = Enumerable.Empty<DirectoryEntry>();

    // Handlers

    public async Task OnGetAsync() => this.DirectoryEntries = await dc.DirectoryEntries.OrderBy(x => x.DisplayName).ToListAsync();

}
