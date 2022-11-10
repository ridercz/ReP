namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.NewsMessageQueryHandlers;
public class NewsMessagesQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessagesQuery, IEnumerable<NewsMessage>>
{
    public NewsMessagesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<NewsMessage>> GetResultToHandleAsync(NewsMessagesQuery query, CancellationToken token)
        => await GetOrderByDescending(nm => nm.Date).ToArrayAsync(cancellationToken: token);
}
