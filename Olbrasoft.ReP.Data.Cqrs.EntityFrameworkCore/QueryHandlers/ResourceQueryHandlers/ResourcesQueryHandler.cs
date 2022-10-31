using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ResourceQueryHandlers;
public class ResourcesQueryHandler : BaseQueryHandler<Resource, ResourcesQuery, IEnumerable<Resource>>
{
    public ResourcesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<Resource>> GetResultToHandleAsync(ResourcesQuery query, CancellationToken token) 
        => query.IsPrivilegedUser
            ? await OrderBy(x => x.Name).ToArrayAsync(token)
            : await Where(x => x.Enabled).OrderBy(x => x.Name).ToArrayAsync(token);
}
