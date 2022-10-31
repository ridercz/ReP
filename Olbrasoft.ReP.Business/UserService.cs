namespace Olbrasoft.ReP.Business;
public class UserService : BaseService, IUserService
{
    public UserService(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public Task<IEnumerable<UserInfoDto>> GetUserInfos(CancellationToken token = default)
        => new UserInfosQuery(Dispatcher).ToResultAsync(token);
}
