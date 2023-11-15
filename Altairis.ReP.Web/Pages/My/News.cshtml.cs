namespace Altairis.ReP.Web.Pages.My;
public class NewsModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc;

    public IEnumerable<NewsMessage> Messages { get; set; } = Enumerable.Empty<NewsMessage>();

    public async Task OnGetAsync() => this.Messages = await this.dc.NewsMessages.OrderByDescending(x => x.Date).ToListAsync();

}
