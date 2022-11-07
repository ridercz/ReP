using Altairis.ReP.Data.Queries.DirectoryEntryQueries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntriesQueryHandler : BaseQueryHandler<DirectoryEntry, DirectoryEntriesQuery, IEnumerable<DirectoryEntry>>
{
    public DirectoryEntriesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<DirectoryEntry>> GetResultToHandleAsync(DirectoryEntriesQuery query, CancellationToken token)
        => await OrderBy(x => x.DisplayName).ToArrayAsync(token);
}