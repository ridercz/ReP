using Altairis.ReP.Data.Dtos;
using NetBox.Extensions;

namespace Altairis.ReP.Web.Pages.My;

public class DirectoryModel : PageModel
{
    private readonly IDirectoryEntryService _service;

    public DirectoryModel(IDirectoryEntryService service) => _service = service ?? throw new ArgumentNullException(nameof(service));

    public IEnumerable<DirectoryEntryInfoDto> Items { get; set; }

    public async Task OnGetAsync(CancellationToken token)
    {
        var userInfos = SetIconClass(await _service.GetDirectoryInfosAsync(true, token), "fas fa-fw fa-user");

        var extraInfos = SetIconClass(await _service.GetDirectoryInfosAsync(token: token), "fas fa-fw fa-address-card");

        this.Items = userInfos.Concat(extraInfos).OrderBy(x => x.DisplayName);
    }

    private static IEnumerable<DirectoryEntryInfoDto> SetIconClass(IEnumerable<DirectoryEntryInfoDto> directoryEntryInfos, string iconClass)
        => directoryEntryInfos.ForEach(deid => deid.IconClass = iconClass);
}
