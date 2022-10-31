using Altairis.ReP.Data.Dtos;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface IUserService
{
    Task<IEnumerable<UserInfoDto>> GetUserInfos(CancellationToken token = default);
}
