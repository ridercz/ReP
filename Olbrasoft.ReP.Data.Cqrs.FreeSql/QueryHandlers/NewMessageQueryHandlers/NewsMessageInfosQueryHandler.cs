namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.NewMessageQueryHandlers;
public class NewsMessageInfosQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageInfosQuery, IEnumerable<NewsMessageInfoDto>>
{
    public NewsMessageInfosQueryHandler(IConfigure<NewsMessage> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<NewsMessageInfoDto>> GetResultToHandleAsync(NewsMessageInfosQuery query, CancellationToken token)
        => await GetEnumerableAsync<NewsMessageInfoDto>(GetOrderBy(nm => nm.Date), token);
}
