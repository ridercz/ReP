namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntriesQueryHandler : RepDbQueryHandler<DirectoryEntry, DirectoryEntriesQuery, IEnumerable<DirectoryEntry>>
{
    public DirectoryEntriesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<DirectoryEntry>> GetResultToHandleAsync(DirectoryEntriesQuery query, CancellationToken token)
        => await GetOrderBy(x => x.DisplayName).ToArrayAsync(token);
}