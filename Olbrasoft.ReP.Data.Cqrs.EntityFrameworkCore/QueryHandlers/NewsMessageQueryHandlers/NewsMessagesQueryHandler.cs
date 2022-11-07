using Altairis.ReP.Data.Queries.NewsMessageQueries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.NewsMessageQueryHandlers;
public class NewsMessagesQueryHandler : BaseQueryHandler<NewsMessage, NewsMessagesQuery, IEnumerable<NewsMessage>>
{
    public NewsMessagesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<NewsMessage>> GetResultToHandleAsync(NewsMessagesQuery query, CancellationToken token)
        => await OrderByDescending(nm => nm.Date).ToArrayAsync(cancellationToken: token);
}
