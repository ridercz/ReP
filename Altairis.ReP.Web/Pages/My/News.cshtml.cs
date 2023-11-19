namespace Altairis.ReP.Web.Pages.My;

public class NewsModel(RepDbContext dc) : PageModel {

    // Output model

    public IEnumerable<NewsMessage> Messages { get; set; } = Enumerable.Empty<NewsMessage>();

    // Handlers

    public async Task OnGetAsync() => this.Messages = await dc.NewsMessages.OrderByDescending(x => x.Date).ToListAsync();

}
