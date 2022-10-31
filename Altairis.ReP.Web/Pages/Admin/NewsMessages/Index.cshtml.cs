using Altairis.ReP.Data.Dtos.NewsMessageDtos;

namespace Altairis.ReP.Web.Pages.Admin.NewsMessages;
public class IndexModel : PageModel
{
    private readonly INewsMessageService _service;

    public IndexModel(INewsMessageService service) 
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    public IEnumerable<NewsMessageInfoDto> NewsMessages { get; set; }

    public async Task OnGetAsync(CancellationToken token) 
        => this.NewsMessages = await _service.GetNewsMessageInfos(token);
}
