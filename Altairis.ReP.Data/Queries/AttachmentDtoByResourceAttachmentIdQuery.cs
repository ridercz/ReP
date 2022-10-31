using Altairis.ReP.Data.Dtos;

namespace Altairis.ReP.Data.Queries;
public class AttachmentDtoByResourceAttachmentIdQuery : BaseQuery<AttachmentDto?>
{
    public AttachmentDtoByResourceAttachmentIdQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResourceAttachmentId { get; set; }
}
