namespace Altairis.ReP.Web.Pages.My;

public class DirectoryModel(RepDbContext dc) : PageModel {

    // Output model
    public record DirectoryEntryInfo(string IconClass, string DisplayName, string? UserName, string? Email, string? PhoneNumber);

    public IEnumerable<DirectoryEntryInfo> Items { get; set; } = Enumerable.Empty<DirectoryEntryInfo>();

    // Handlers

    public async Task OnGetAsync() {
        var userInfoQuery = from u in dc.Users
                            where u.ShowInMemberDirectory
                            select new DirectoryEntryInfo("fas fa-fw fa-user", u.DisplayName, u.UserName, u.Email, u.PhoneNumber);
        var extraInfoQuery = from de in dc.DirectoryEntries
                             select new DirectoryEntryInfo("fas fa-fw fa-address-card", de.DisplayName, null, de.Email, de.PhoneNumber);
        var userInfos = await userInfoQuery.ToListAsync();
        var extraInfos = await extraInfoQuery.ToListAsync();
        this.Items = userInfos.Concat(extraInfos).OrderBy(x => x.DisplayName);
    }

}
