namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ResourceQueryHandlers;
public class ResourcesQueryHandler : RepDbQueryHandler<Resource, ResourcesQuery, IEnumerable<Resource>>
{
    public ResourcesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<Resource>> GetResultToHandleAsync(ResourcesQuery query, CancellationToken token) 
        => query.IsPrivilegedUser
            ? await GetOrderBy(x => x.Name).ToArrayAsync(token)
            : await GetWhere(x => x.Enabled).OrderBy(x => x.Name).ToArrayAsync(token);
}
