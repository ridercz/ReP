using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries.DirectoryEntryQueries;
public class DirectoryEntryQuery : BaseQuery<DirectoryEntry?>
{
    public DirectoryEntryQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int DirectoryEntryId { get; set; }
}
