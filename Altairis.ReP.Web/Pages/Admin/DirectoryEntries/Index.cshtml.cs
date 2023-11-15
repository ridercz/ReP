namespace Altairis.ReP.Web.Pages.Admin.DirectoryEntries;

public class IndexModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc ?? throw new ArgumentNullException(nameof(dc));

    public IEnumerable<DirectoryEntry> DirectoryEntries { get; set; }

    public async Task OnGetAsync() {
        this.DirectoryEntries = await this.dc.DirectoryEntries.OrderBy(x => x.DisplayName).ToListAsync();
    }

}
