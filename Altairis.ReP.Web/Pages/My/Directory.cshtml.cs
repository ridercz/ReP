using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ReP.Web.Pages.My;

public class DirectoryModel : PageModel {
    private readonly RepDbContext dc;

    public DirectoryModel(RepDbContext dc) {
        this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
    }

    public IEnumerable<ApplicationUser> Items { get; set; }

    public async Task OnGetAsync() {
        this.Items = await this.dc.Users.Where(x => x.ShowInMemberDirectory).OrderBy(x => x.DisplayName).ToListAsync();
    }
}
