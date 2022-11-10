namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ResourceQueryHandlers;
public class ResourceInfoQueryHandler : RepDbQueryHandler<Resource, ResourceInfosQuery, IEnumerable<ResourceInfoDto>>
{
    public ResourceInfoQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<ResourceInfoDto>> GetResultToHandleAsync(ResourceInfosQuery query, CancellationToken token)
        => await ProjectTo<ResourceInfoDto>(GetOrderBy(r => r.Name)).ToArrayAsync(token);
}
