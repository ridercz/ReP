using Altairis.ReP.Data.Dtos.NewsMessageDtos;
using Altairis.ReP.Data.Queries.NewsMessageQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.NewMessageQueryHandlers;
public class NewsMessageInfosQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageInfosQuery, IEnumerable<NewsMessageInfoDto>>
{
    public NewsMessageInfosQueryHandler(IConfigure<NewsMessage> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {
    }

    protected override async Task<IEnumerable<NewsMessageInfoDto>> GetResultToHandleAsync(NewsMessageInfosQuery query, CancellationToken token)
        => await GetEnumerableAsync<NewsMessageInfoDto>(GetOrderBy(nm => nm.Date), token);
}
