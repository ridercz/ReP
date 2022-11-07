using Altairis.ReP.Data.Queries.DirectoryEntryQueries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntryQueryHandler : BaseQueryHandler<DirectoryEntry, DirectoryEntryQuery, DirectoryEntry?>
{
    public DirectoryEntryQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<DirectoryEntry?> GetResultToHandleAsync(DirectoryEntryQuery query, CancellationToken token)
        => await SingleOrDefaultAsync(de => de.Id == query.DirectoryEntryId, token);
}
