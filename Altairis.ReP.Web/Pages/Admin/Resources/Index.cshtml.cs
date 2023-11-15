namespace Altairis.ReP.Web.Pages.Admin.Resources;
public class IndexModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc ?? throw new ArgumentNullException(nameof(dc));

    public IEnumerable<ResourceInfo> Resources { get; set; }

    public class ResourceInfo {
        public string Description { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string ForegroundColor { get; set; }
        public string BackgroundColor { get; set; }

        public string GetStyle() => $"color:{this.ForegroundColor};background-color:{this.BackgroundColor};";
    }

    public async Task OnGetAsync() {
        var q = from r in this.dc.Resources
                orderby r.Name
                select new ResourceInfo {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    ForegroundColor = r.ForegroundColor,
                    BackgroundColor = r.BackgroundColor
                };
        this.Resources = await q.ToListAsync();
    }

}
