using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ResourceQueryHandlers;
public class ResourceInfoQueryHandler : BaseQueryHandlerWithProjector<Resource, ResourceInfosQuery, IEnumerable<ResourceInfoDto>>
{
    public ResourceInfoQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<ResourceInfoDto>> GetResultToHandleAsync(ResourceInfosQuery query, CancellationToken token)
        => await ProjectTo<ResourceInfoDto>(OrderBy(r => r.Name)).ToArrayAsync(token);
}
