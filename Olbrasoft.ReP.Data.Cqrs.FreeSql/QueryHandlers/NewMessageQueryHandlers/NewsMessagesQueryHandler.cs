using Altairis.ReP.Data.Queries.NewsMessageQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.NewMessageQueryHandlers;
public class NewsMessagesQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessagesQuery, IEnumerable<NewsMessage>>
{
    public NewsMessagesQueryHandler(IConfigure<NewsMessage> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {
    }

    protected override async Task<IEnumerable<NewsMessage>> GetResultToHandleAsync(NewsMessagesQuery query, CancellationToken token)
        => await GetEnumerableAsync(GetOrderByDescending(nm => nm.Date), token);
}
