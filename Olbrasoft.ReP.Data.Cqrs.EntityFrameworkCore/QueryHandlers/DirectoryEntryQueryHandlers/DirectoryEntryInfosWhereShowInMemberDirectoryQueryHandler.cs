namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntryInfosWhereShowInMemberDirectoryQueryHandler
    : RepDbQueryHandler<ApplicationUser, DirectoryEntryInfosWhereShowInMemberDirectoryQuery, IEnumerable<DirectoryEntryInfoDto>>
{
    public DirectoryEntryInfosWhereShowInMemberDirectoryQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<DirectoryEntryInfoDto>> GetResultToHandleAsync(DirectoryEntryInfosWhereShowInMemberDirectoryQuery query,
                                                                                           CancellationToken token)
        => await GetEnumerableAsync<DirectoryEntryInfoDto>(GetWhere(u => u.ShowInMemberDirectory), token);
}
