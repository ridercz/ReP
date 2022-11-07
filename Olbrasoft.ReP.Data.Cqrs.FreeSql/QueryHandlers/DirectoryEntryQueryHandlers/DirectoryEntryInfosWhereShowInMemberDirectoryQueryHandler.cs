using Altairis.ReP.Data.Dtos;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.DirectoryEntryQueryHandlers;
public class DirectoryEntryInfosWhereShowInMemberDirectoryQueryHandler
    : RepDbQueryHandler<ApplicationUser, DirectoryEntryInfosWhereShowInMemberDirectoryQuery, IEnumerable<DirectoryEntryInfoDto>>
{
    public DirectoryEntryInfosWhereShowInMemberDirectoryQueryHandler(IConfigure<ApplicationUser> projectionConfigurator, IDataSelector selector) 
        : base(projectionConfigurator, selector)
    {}

    protected override async Task<IEnumerable<DirectoryEntryInfoDto>> GetResultToHandleAsync(DirectoryEntryInfosWhereShowInMemberDirectoryQuery query,
                                                                                             CancellationToken token)
        => await GetEnumerableAsync<DirectoryEntryInfoDto>(GetWhere(u => u.ShowInMemberDirectory), token);
}
