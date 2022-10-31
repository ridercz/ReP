using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class NewsletterRecipientsQueryHandler : BaseQueryHandlerWithProjector<ApplicationUser, NewsletterRecipientsQuery, IEnumerable<RecipientDto>>
{
    public NewsletterRecipientsQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<RecipientDto>> GetResultToHandleAsync(NewsletterRecipientsQuery query, CancellationToken token) 
        => await ProjectTo<RecipientDto>(Where(p => p.SendNews)).ToArrayAsync(token);
}
