using Altairis.ReP.Data.Dtos;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.UserQueryHandlers;
public class UserInfosQueryHandler : BaseQueryHandlerWithProjector<ApplicationUser, UserInfosQuery, IEnumerable<UserInfoDto>>
{
    public UserInfosQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<UserInfoDto>> GetResultToHandleAsync(UserInfosQuery query, CancellationToken token)
        => await ProjectTo<UserInfoDto>(OrderBy(u => u.UserName)).ToArrayAsync(token);
}
