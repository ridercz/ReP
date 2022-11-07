using Altairis.ReP.Data.Dtos;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;
public class NewsletterRecipientsQueryHandler : RepDbQueryHandler<ApplicationUser, NewsletterRecipientsQuery, IEnumerable<RecipientDto>>
{
    public NewsletterRecipientsQueryHandler(IConfigure<ApplicationUser> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {
    }

    protected override async Task<IEnumerable<RecipientDto>> GetResultToHandleAsync(NewsletterRecipientsQuery query, CancellationToken token)
        => await GetEnumerableAsync<RecipientDto>(GetWhere(p => p.SendNews),token);
}
