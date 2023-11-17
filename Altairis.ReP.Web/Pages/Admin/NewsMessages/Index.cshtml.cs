namespace Altairis.ReP.Web.Pages.Admin.NewsMessages;
public class IndexModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc;

    // Output model

    public IEnumerable<NewsMessageInfo> NewsMessages { get; set; } = Enumerable.Empty<NewsMessageInfo>();

    public record NewsMessageInfo(int Id, DateTime Date, string Title);

    // Handlers

    public async Task OnGetAsync() {
        var q = from m in this.dc.NewsMessages
                orderby m.Date descending
                select new NewsMessageInfo(m.Id, m.Date, m.Title);
        this.NewsMessages = await q.ToListAsync();
    }

}
