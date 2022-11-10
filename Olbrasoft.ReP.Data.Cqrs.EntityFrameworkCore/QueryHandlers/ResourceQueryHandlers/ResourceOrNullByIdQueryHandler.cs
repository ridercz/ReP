namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ResourceQueryHandlers;
public class ResourceOrNullByIdQueryHandler : RepDbQueryHandler<Resource, ResourceOrNullByIdQuery, Resource?>
{
    public ResourceOrNullByIdQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<Resource?> GetResultToHandleAsync(ResourceOrNullByIdQuery query, CancellationToken token)
        => await GetOneOrNullAsync(r => r.Id == query.ResourceId, token);
}
