namespace Altairis.ReP.Web.Pages.My;
public class NewsModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc ?? throw new ArgumentNullException(nameof(dc));

    public IEnumerable<NewsMessage> Messages { get; set; }

    public async Task OnGetAsync() {
        this.Messages = await this.dc.NewsMessages.OrderByDescending(x => x.Date).ToListAsync();
    }

}
