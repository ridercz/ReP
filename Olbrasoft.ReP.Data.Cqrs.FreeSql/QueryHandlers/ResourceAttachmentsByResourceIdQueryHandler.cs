namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;
public class ResourceAttachmentsByResourceIdQueryHandler :
    RepDbQueryHandler<ResourceAttachment, ResourceAttachmentsByResourceIdQuery, IEnumerable<ResourceAttachment>>
{
    public ResourceAttachmentsByResourceIdQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected override async Task<IEnumerable<ResourceAttachment>> GetResultToHandleAsync(
        ResourceAttachmentsByResourceIdQuery query,
        CancellationToken token)
        => await GetEnumerableAsync(
            GetWhere(ra => ra.ResourceId == query.ResourceId).OrderByDescending(x => x.DateCreated), token);
}