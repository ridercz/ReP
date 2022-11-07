using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Queries.UserQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.UserQueryHandlers;
public class UserInfosQueryHandler : RepDbQueryHandler<ApplicationUser, UserInfosQuery, IEnumerable<UserInfoDto>>
{
    public UserInfosQueryHandler(IConfigure<ApplicationUser> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {
    }

    protected override async Task<IEnumerable<UserInfoDto>> GetResultToHandleAsync(UserInfosQuery query, CancellationToken token)
        => await GetEnumerableAsync<UserInfoDto>(GetOrderBy(u => u.UserName), token);
}
