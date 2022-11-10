namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.UserQueryHandlers;
public class UserInfosQueryHandler : RepDbQueryHandler<ApplicationUser, UserInfosQuery, IEnumerable<UserInfoDto>>
{
    public UserInfosQueryHandler(IConfigure<ApplicationUser> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<UserInfoDto>> GetResultToHandleAsync(UserInfosQuery query, CancellationToken token)
        => await GetEnumerableAsync<UserInfoDto>(GetOrderBy(u => u.UserName), token);
}
