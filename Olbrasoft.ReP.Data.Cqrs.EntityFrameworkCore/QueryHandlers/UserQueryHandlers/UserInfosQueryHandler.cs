namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.UserQueryHandlers;
public class UserInfosQueryHandler : RepDbQueryHandler<ApplicationUser, UserInfosQuery, IEnumerable<UserInfoDto>>
{
    public UserInfosQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<UserInfoDto>> GetResultToHandleAsync(UserInfosQuery query, CancellationToken token)
        => await ProjectTo<UserInfoDto>(GetOrderBy(u => u.UserName)).ToArrayAsync(token);
}
