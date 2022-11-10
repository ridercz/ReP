namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntryInfosQueryHandler : RepDbQueryHandler<DirectoryEntry, DirectoryEntryInfosQuery, IEnumerable<DirectoryEntryInfoDto>>
{
    public DirectoryEntryInfosQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<DirectoryEntryInfoDto>> GetResultToHandleAsync(DirectoryEntryInfosQuery query, CancellationToken token)
        => await GetEnumerableAsync<DirectoryEntryInfoDto>(token);
}
