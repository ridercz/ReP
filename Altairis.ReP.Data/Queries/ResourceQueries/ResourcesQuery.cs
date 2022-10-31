using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries.ResourceQueries;
public class ResourcesQuery : BaseQuery<IEnumerable<Resource>>
{
    public ResourcesQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public bool IsPrivilegedUser { get; set; }
}
