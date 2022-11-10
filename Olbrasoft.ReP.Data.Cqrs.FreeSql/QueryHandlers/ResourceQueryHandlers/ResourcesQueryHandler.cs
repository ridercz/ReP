namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ResourceQueryHandlers;
public class ResourcesQueryHandler : RepDbQueryHandler<Resource, ResourcesQuery, IEnumerable<Resource>>
{
    public ResourcesQueryHandler(IConfigure<Resource> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<Resource>> GetResultToHandleAsync(ResourcesQuery query, CancellationToken token)
        => query.IsPrivilegedUser
                 ? await GetEnumerableAsync(GetOrderBy(x => x.Name), token)
                 : await GetEnumerableAsync(GetWhere(x => x.Enabled).OrderBy(x => x.Name), token);

}
