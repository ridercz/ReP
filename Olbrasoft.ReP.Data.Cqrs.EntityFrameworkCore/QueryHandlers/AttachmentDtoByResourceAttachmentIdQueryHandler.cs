namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class AttachmentDtoByResourceAttachmentIdQueryHandler : RepDbQueryHandler<ResourceAttachment, AttachmentDtoByResourceAttachmentIdQuery, AttachmentDto?>
{
    public AttachmentDtoByResourceAttachmentIdQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<AttachmentDto?> GetResultToHandleAsync(AttachmentDtoByResourceAttachmentIdQuery query, CancellationToken token) 
        => await ProjectTo<AttachmentDto>(Queryable.Where(ra => ra.Id == query.ResourceAttachmentId)).FirstOrDefaultAsync(token);
}
