using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class ResourceAttachmentsByResourceIdQueryHandler :
    RepDbQueryHandler<ResourceAttachment, ResourceAttachmentsByResourceIdQuery, IEnumerable<ResourceAttachment>>
{
    public ResourceAttachmentsByResourceIdQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<ResourceAttachment>> GetResultToHandleAsync(
        ResourceAttachmentsByResourceIdQuery query,
        CancellationToken token)
        => await GetWhere(ra => ra.ResourceId == query.ResourceId)
                                    .OrderByDescending(x => x.DateCreated)
                                    .ToArrayAsync(token);
}