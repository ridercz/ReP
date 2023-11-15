namespace Altairis.ReP.Web.Pages.Admin.Resources;
public class IndexModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc;

    public IEnumerable<ResourceInfo> Resources { get; set; } = new List<ResourceInfo>();

    public record ResourceInfo(string? Description, string Name, int Id, string ForegroundColor, string BackgroundColor) {
        public string GetStyle() => $"color:{this.ForegroundColor};background-color:{this.BackgroundColor};";
    }

    public async Task OnGetAsync() {
        var q = from r in this.dc.Resources
                orderby r.Name
                select new ResourceInfo(r.Description, r.Name, r.Id, r.ForegroundColor, r.BackgroundColor);
        this.Resources = await q.ToListAsync();
    }

}
