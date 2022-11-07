namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntryQueryHandler : RepDbQueryHandler<DirectoryEntry, DirectoryEntryQuery, DirectoryEntry?>
{
    public DirectoryEntryQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    protected override async Task<DirectoryEntry?> GetResultToHandleAsync(DirectoryEntryQuery query, CancellationToken token)
        => await GetOneOrNullAsync(de => de.Id == query.DirectoryEntryId, token);
}
