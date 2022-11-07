namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;
public class ResourceAttachmentsByResourceIdQueryHandler :
    RepDbQueryHandler<ResourceAttachment, ResourceAttachmentsByResourceIdQuery, IEnumerable<ResourceAttachment>>
{
    public ResourceAttachmentsByResourceIdQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    protected override async Task<IEnumerable<ResourceAttachment>> GetResultToHandleAsync(
        ResourceAttachmentsByResourceIdQuery query,
        CancellationToken token)
        => await GetWhere(ra => ra.ResourceId == query.ResourceId)
                                    .OrderByDescending(x => x.DateCreated)
                                    .ToListAsync(token);
}