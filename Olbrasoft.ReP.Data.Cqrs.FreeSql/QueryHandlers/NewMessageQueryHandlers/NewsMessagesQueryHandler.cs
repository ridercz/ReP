namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.NewMessageQueryHandlers;
public class NewsMessagesQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessagesQuery, IEnumerable<NewsMessage>>
{
    public NewsMessagesQueryHandler(IConfigure<NewsMessage> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<NewsMessage>> GetResultToHandleAsync(NewsMessagesQuery query, CancellationToken token)
        => await GetEnumerableAsync(GetOrderByDescending(nm => nm.Date), token);
}
