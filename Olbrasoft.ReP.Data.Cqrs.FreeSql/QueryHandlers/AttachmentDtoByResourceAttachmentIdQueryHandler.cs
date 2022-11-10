namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;
public class AttachmentDtoByResourceAttachmentIdQueryHandler : RepDbQueryHandler<ResourceAttachment, AttachmentDtoByResourceAttachmentIdQuery, AttachmentDto?>
{
    public AttachmentDtoByResourceAttachmentIdQueryHandler(IConfigure<ResourceAttachment> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<AttachmentDto?> GetResultToHandleAsync(AttachmentDtoByResourceAttachmentIdQuery query, CancellationToken token)
        => await GetOneOrNullAsync<AttachmentDto>(ra => ra.Id == query.ResourceAttachmentId, token);
}
