using Altairis.ReP.Data.Dtos.ResourceDtos;

namespace Altairis.ReP.Data.Queries.ResourceQueries;
public class ResourceTagsQuery : BaseQuery<IEnumerable<ResourceTagDto>>
{
    public ResourceTagsQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
