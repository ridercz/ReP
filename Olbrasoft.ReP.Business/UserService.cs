using Altairis.ReP.Data.Queries.UserQueries;

namespace Olbrasoft.ReP.Business;
public class UserService : BaseService, IUserService
{
    public UserService(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public Task<IEnumerable<UserInfoDto>> GetUserInfosAsync(CancellationToken token = default)
        => new UserInfosQuery(Dispatcher).ToResultAsync(token);

    public Task<bool> IsThereAnyUserAsync(CancellationToken token = default) => new IsThereAnyUserQuery(Dispatcher).ToResultAsync(token);
}
