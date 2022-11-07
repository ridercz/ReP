using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class AttachmentDtoByResourceAttachmentIdQueryHandler : BaseQueryHandlerWithProjector<ResourceAttachment, AttachmentDtoByResourceAttachmentIdQuery, AttachmentDto?>
{
    public AttachmentDtoByResourceAttachmentIdQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<AttachmentDto?> GetResultToHandleAsync(AttachmentDtoByResourceAttachmentIdQuery query, CancellationToken token) 
        => await ProjectTo<AttachmentDto>(EntityQueryable.Where(ra => ra.Id == query.ResourceAttachmentId)).FirstOrDefaultAsync(token);
}
