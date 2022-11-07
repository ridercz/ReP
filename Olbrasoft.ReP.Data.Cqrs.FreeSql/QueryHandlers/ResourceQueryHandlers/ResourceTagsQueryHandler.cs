using Altairis.ReP.Data.Dtos;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ResourceQueryHandlers;
public class ResourceTagsQueryHandler : RepDbQueryHandler<Resource, ResourceTagsQuery, IEnumerable<ResourceTagDto>>
{
    public ResourceTagsQueryHandler(IConfigure<Resource> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {}

    protected override async Task<IEnumerable<ResourceTagDto>> GetResultToHandleAsync(ResourceTagsQuery query, CancellationToken token)
        => await GetEnumerableAsync<ResourceTagDto>(GetOrderBy(r => r.Name), token);
}