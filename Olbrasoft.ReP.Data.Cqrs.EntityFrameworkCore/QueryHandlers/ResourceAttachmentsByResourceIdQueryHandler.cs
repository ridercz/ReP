using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class ResourceAttachmentsByResourceIdQueryHandler :
    BaseQueryHandler<ResourceAttachment, ResourceAttachmentsByResourceIdQuery, IEnumerable<ResourceAttachment>>
{
    public ResourceAttachmentsByResourceIdQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<ResourceAttachment>> GetResultToHandleAsync(
        ResourceAttachmentsByResourceIdQuery query,
        CancellationToken token)
        => await Where(ra => ra.ResourceId == query.ResourceId)
                                    .OrderByDescending(x => x.DateCreated)
                                    .ToArrayAsync(token);
}