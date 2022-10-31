using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Web.Pages.Admin.DirectoryEntries;

public class IndexModel : PageModel {
    private readonly IDirectoryEntryService _service;

    public IndexModel(IDirectoryEntryService service) 
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    public IEnumerable<DirectoryEntry> DirectoryEntries { get; set; }

    public async Task OnGetAsync(CancellationToken token) 
        => this.DirectoryEntries = await _service.GetDirectoryEntriesAsync(token);
}
