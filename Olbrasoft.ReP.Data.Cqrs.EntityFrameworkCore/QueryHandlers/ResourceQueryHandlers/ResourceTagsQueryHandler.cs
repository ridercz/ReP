using Altairis.ReP.Data.Dtos;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ResourceQueryHandlers;
public class ResourceTagsQueryHandler : BaseQueryHandlerWithProjector<Resource, ResourceTagsQuery, IEnumerable<ResourceTagDto>>
{
    public ResourceTagsQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<ResourceTagDto>> GetResultToHandleAsync(ResourceTagsQuery query, CancellationToken token) 
        => await ProjectTo<ResourceTagDto>(OrderBy(r => r.Name)).ToArrayAsync(token);
}
