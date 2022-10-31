using Altairis.ReP.Data.Dtos;

namespace Altairis.ReP.Data.Queries.ResourceQueries;
public class ResourceInfosQuery : BaseQuery<IEnumerable<ResourceInfoDto>>
{
    public ResourceInfosQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
