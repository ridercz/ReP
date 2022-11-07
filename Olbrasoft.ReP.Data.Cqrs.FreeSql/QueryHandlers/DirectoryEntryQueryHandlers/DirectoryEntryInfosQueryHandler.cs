using Altairis.ReP.Data.Dtos;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntryInfosQueryHandler : RepDbQueryHandler<DirectoryEntry, DirectoryEntryInfosQuery, IEnumerable<DirectoryEntryInfoDto>>
{
    public DirectoryEntryInfosQueryHandler(IConfigure<DirectoryEntry> configurator, IDataSelector selector)
        : base(configurator, selector)
    {}

    protected override async Task<IEnumerable<DirectoryEntryInfoDto>> GetResultToHandleAsync(DirectoryEntryInfosQuery query, CancellationToken token)
        => await GetEnumerableAsync<DirectoryEntryInfoDto>(token);
}
