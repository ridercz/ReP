using Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore;

namespace Altairis.ReP.Web.Pages.My;

public class DirectoryModel : PageModel {
    private readonly RepDbContext dc;

    public DirectoryModel(RepDbContext dc) {
        this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
    }

    public class DirectoryEntryInfo {
        public string IconClass { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public IEnumerable<DirectoryEntryInfo> Items { get; set; }

    public async Task OnGetAsync() {
        var userInfoQuery = from u in this.dc.Users
                            where u.ShowInMemberDirectory
                            select new DirectoryEntryInfo {
                                IconClass = "fas fa-fw fa-user",
                                DisplayName = u.DisplayName,
                                UserName = u.UserName,
                                Email = u.Email,
                                PhoneNumber = u.PhoneNumber
                            };
        var extraInfoQuery = from de in this.dc.DirectoryEntries
                             select new DirectoryEntryInfo {
                                 IconClass = "fas fa-fw fa-address-card",
                                 DisplayName = de.DisplayName,
                                 Email = de.Email,
                                 PhoneNumber = de.PhoneNumber
                             };
        var userInfos = await userInfoQuery.ToListAsync();
        var extraInfos = await extraInfoQuery.ToListAsync();
        this.Items = userInfos.Concat(extraInfos).OrderBy(x => x.DisplayName);
    }
}
