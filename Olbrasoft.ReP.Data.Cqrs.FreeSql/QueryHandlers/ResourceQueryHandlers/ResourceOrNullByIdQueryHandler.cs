namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ResourceQueryHandlers;
public class ResourceOrNullByIdQueryHandler : RepDbQueryHandler<Resource, ResourceOrNullByIdQuery, Resource?>
{
    public ResourceOrNullByIdQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected override async Task<Resource?> GetResultToHandleAsync(ResourceOrNullByIdQuery query, CancellationToken token)
        => await GetOneOrNullAsync(r => r.Id == query.ResourceId ,token);
}
