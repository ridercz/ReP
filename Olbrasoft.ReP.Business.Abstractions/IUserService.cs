using Altairis.ReP.Data.Dtos;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface IUserService
{
    Task<IEnumerable<UserInfoDto>> GetUserInfosAsync(CancellationToken token = default);
    Task<bool> IsThereAnyUserAsync(CancellationToken token = default);
}
