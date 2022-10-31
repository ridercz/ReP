namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ResourceQueryHandlers;
public class ResourcesQueryHandler : RepDbQueryHandler<Resource, ResourcesQuery, IEnumerable<Resource>>
{
    public ResourcesQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    protected override async Task<IEnumerable<Resource>> GetResultToHandleAsync(ResourcesQuery query, CancellationToken token)
    {
        return query.IsPrivilegedUser
                 ? await OrderBy(x => x.Name).ToListAsync(token)
                 : await Where(x => x.Enabled).OrderBy(x => x.Name).ToListAsync(token);
    }

}
