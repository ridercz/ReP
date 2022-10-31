using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ResourceQueryHandlers;
public class ResourceOrNullByIdQueryHandler : BaseQueryHandler<Resource, ResourceOrNullByIdQuery, Resource?>
{
    public ResourceOrNullByIdQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<Resource?> GetResultToHandleAsync(ResourceOrNullByIdQuery query, CancellationToken token)
        => await SingleOrDefaultAsync(r => r.Id == query.ResourceId, token);
}
