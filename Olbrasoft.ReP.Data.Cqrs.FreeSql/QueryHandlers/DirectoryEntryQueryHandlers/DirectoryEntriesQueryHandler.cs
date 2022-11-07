namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntriesQueryHandler : RepDbQueryHandler<DirectoryEntry, DirectoryEntriesQuery, IEnumerable<DirectoryEntry>>
{
    public DirectoryEntriesQueryHandler(IConfigure<DirectoryEntry> projectionConfigurator, IDataSelector selector)
        : base(projectionConfigurator, selector)
    {
    }

    protected override async Task<IEnumerable<DirectoryEntry>> GetResultToHandleAsync(DirectoryEntriesQuery query, CancellationToken token)
        => await GetEnumerableAsync(GetOrderBy(x => x.DisplayName), token);
}