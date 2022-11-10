namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntryInfosQueryHandler : RepDbQueryHandler<DirectoryEntry, DirectoryEntryInfosQuery, IEnumerable<DirectoryEntryInfoDto>>
{
    public DirectoryEntryInfosQueryHandler(IConfigure<DirectoryEntry> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<DirectoryEntryInfoDto>> GetResultToHandleAsync(DirectoryEntryInfosQuery query, CancellationToken token)
        => await GetEnumerableAsync<DirectoryEntryInfoDto>(token);
}
