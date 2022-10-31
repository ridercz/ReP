using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries;
public class ResourceAttachmentsByResourceIdQuery : BaseQuery<IEnumerable<ResourceAttachment>>
{
    public ResourceAttachmentsByResourceIdQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResourceId { get; set; }
}