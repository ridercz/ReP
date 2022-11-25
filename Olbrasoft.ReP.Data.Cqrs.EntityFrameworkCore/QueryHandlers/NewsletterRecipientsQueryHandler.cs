namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class NewsletterRecipientsQueryHandler : RepDbQueryHandler<ApplicationUser, NewsletterRecipientsQuery, IEnumerable<RecipientDto>>
{
    public NewsletterRecipientsQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<RecipientDto>> GetResultToHandleAsync(NewsletterRecipientsQuery query, CancellationToken token) 
        => await ProjectTo<RecipientDto>(GetWhere(p => p.SendNews)).ToArrayAsync(token);
}
