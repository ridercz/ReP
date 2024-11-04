namespace Altairis.ReP.Web.Pages.Admin.Resources.Tasks;

public class IndexModel(RepDbContext dc) : PageModel
{
    // Output model

    public string ResourceName { get; set; } = string.Empty;

    // Handlers

    public async Task<IActionResult> OnGetAsync(int resourceId) => (await this.InitAsync(resourceId)) ? this.Page() : this.NotFound();

    // Helpers

    public async Task<bool> InitAsync(int resourceId) {
        var resource = await dc.Resources.FindAsync(resourceId);
        if (resource == null) return false;
        this.ResourceName = resource.Name;
        return true;
    }
}
