using Altairis.ReP.Data.Dtos;

namespace Altairis.ReP.Data.Queries.DirectoryEntryQueries;
public class DirectoryEntryInfosWhereShowInMemberDirectoryQuery : BaseQuery<IEnumerable<DirectoryEntryInfoDto>>
{
    public DirectoryEntryInfosWhereShowInMemberDirectoryQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
