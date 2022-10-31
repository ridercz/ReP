using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries.ResourceQueries;
public class ResourceOrNullByIdQuery : BaseQuery<Resource?>
{
    public int ResourceId { get; set; }

    public ResourceOrNullByIdQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}