using Altairis.ReP.Data.Dtos;

namespace Altairis.ReP.Web.Pages.Admin.Resources;
public class IndexModel : PageModel 
{
    private readonly IResourceService _service;

    public IndexModel(IResourceService service) 
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    public IEnumerable<ResourceInfoDto> Resources { get; set; }

    public async Task OnGetAsync(CancellationToken token)
        => this.Resources = await _service.GetResourceInfosAsync(token);
}
