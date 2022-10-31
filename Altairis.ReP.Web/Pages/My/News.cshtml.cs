using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Web.Pages.My;
public class NewsModel : PageModel
{
    private readonly INewsMessageService _service;

    public NewsModel(INewsMessageService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public IEnumerable<NewsMessage> Messages { get; set; }

    public async Task OnGetAsync(CancellationToken token)
    {
        this.Messages = await _service.GetNewsMessagesAsync(token);
    }

}
