namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ResourceQueryHandlers;
public class ResourceOrNullByIdQueryHandler : RepDbQueryHandler<Resource, ResourceOrNullByIdQuery, Resource?>
{
    public ResourceOrNullByIdQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    protected override async Task<Resource?> GetResultToHandleAsync(ResourceOrNullByIdQuery query, CancellationToken token)
        => await GetOneOrNullAsync(GetWhere(r => r.Id == query.ResourceId),token);
}
