namespace Altairis.ReP.Web.Pages.Admin.Users;

public class IndexModel(RepDbContext dc) : PageModel {
    private readonly RepDbContext dc = dc;

    // Output model

    public IEnumerable<UserInfo> Users { get; set; } = new List<UserInfo>();

    public record UserInfo(int Id, string UserName, string DisplayName, string Email, string Language, string? PhoneNumber, bool Enabled, bool EmailConfirmed);

    // Handlers

    public async Task OnGetAsync() {
        var q = from u in this.dc.Users
                orderby u.UserName
                select new UserInfo(u.Id, u.UserName ?? string.Empty, u.DisplayName, u.Email ?? string.Empty, u.Language, u.PhoneNumber, u.Enabled, u.EmailConfirmed);
        this.Users = await q.ToListAsync();
    }

}
