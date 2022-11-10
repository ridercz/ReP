namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;
public class NewsletterRecipientsQueryHandler : RepDbQueryHandler<ApplicationUser, NewsletterRecipientsQuery, IEnumerable<RecipientDto>>
{
    public NewsletterRecipientsQueryHandler(IConfigure<ApplicationUser> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<RecipientDto>> GetResultToHandleAsync(NewsletterRecipientsQuery query, CancellationToken token)
        => await GetEnumerableAsync<RecipientDto>(p => p.SendNews, token);
}
