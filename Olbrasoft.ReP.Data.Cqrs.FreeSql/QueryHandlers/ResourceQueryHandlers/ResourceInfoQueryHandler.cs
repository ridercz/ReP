using Altairis.ReP.Data.Dtos.ResourceDtos;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ResourceQueryHandlers;
public class ResourceInfoQueryHandler : RepDbQueryHandler<Resource, ResourceInfosQuery, IEnumerable<ResourceInfoDto>>
{
    public ResourceInfoQueryHandler(IConfigure<Resource> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<ResourceInfoDto>> GetResultToHandleAsync(ResourceInfosQuery query, CancellationToken token)
        => await GetEnumerableAsync<ResourceInfoDto>(GetOrderBy(r => r.Name), token);
}
