using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class UserInfosQueryHandler : BaseQueryHandlerWithProjector<ApplicationUser, UserInfosQuery, IEnumerable<UserInfoDto>>
{
    public UserInfosQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<UserInfoDto>> GetResultToHandleAsync(UserInfosQuery query, CancellationToken token) 
        => await ProjectTo<UserInfoDto>(OrderBy(u => u.UserName)).ToArrayAsync(token);
}
