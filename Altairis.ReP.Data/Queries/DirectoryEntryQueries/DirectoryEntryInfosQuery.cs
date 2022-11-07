using Altairis.ReP.Data.Dtos;

namespace Altairis.ReP.Data.Queries.DirectoryEntryQueries;
public class DirectoryEntryInfosQuery : BaseQuery<IEnumerable<DirectoryEntryInfoDto>>
{
    public DirectoryEntryInfosQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
