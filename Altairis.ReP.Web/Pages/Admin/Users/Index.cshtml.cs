using Altairis.ReP.Data.Dtos;

namespace Altairis.ReP.Web.Pages.Admin.Users;
public class IndexModel : PageModel {
    private readonly IUserService _service;

    public IndexModel(IUserService service) 
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    public IEnumerable<UserInfoDto> Users { get; set; }

    public async Task OnGetAsync(CancellationToken token)
        => this.Users = await _service.GetUserInfosAsync(token);

}
