namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.NewMessageQueryHandlers;
public class NewsMessageOrNullByIdQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageOrNullByIdQuery, NewsMessageDto?>
{
    public NewsMessageOrNullByIdQueryHandler(IConfigure<NewsMessage> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<NewsMessageDto?> GetResultToHandleAsync(NewsMessageOrNullByIdQuery query, CancellationToken token)
        => await GetOneOrNullAsync<NewsMessageDto>(nm => nm.Id == query.NewsMessageId,token);
}
