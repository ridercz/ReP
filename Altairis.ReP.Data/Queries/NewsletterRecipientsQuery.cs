using Altairis.ReP.Data.Dtos;

namespace Altairis.ReP.Data.Queries;

public class NewsletterRecipientsQuery : BaseQuery<IEnumerable<RecipientDto>>
{
    public NewsletterRecipientsQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
