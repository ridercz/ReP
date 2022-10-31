using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class NewsMessagesQueryHandler : BaseQueryHandler<NewsMessage, NewsMessagesQuery, IEnumerable<NewsMessage>>
{
    public NewsMessagesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<NewsMessage>> GetResultToHandleAsync(NewsMessagesQuery query, CancellationToken token)
        => await OrderByDescending(nm => nm.Date).ToListAsync(cancellationToken: token);
}
