namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntryInfosWhereShowInMemberDirectoryQueryHandler
    : RepDbQueryHandler<ApplicationUser, DirectoryEntryInfosWhereShowInMemberDirectoryQuery, IEnumerable<DirectoryEntryInfoDto>>
{
    public DirectoryEntryInfosWhereShowInMemberDirectoryQueryHandler(IConfigure<ApplicationUser> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<DirectoryEntryInfoDto>> GetResultToHandleAsync(DirectoryEntryInfosWhereShowInMemberDirectoryQuery query,
                                                                                             CancellationToken token)
        => await GetEnumerableAsync<DirectoryEntryInfoDto>(u => u.ShowInMemberDirectory, token);
}
