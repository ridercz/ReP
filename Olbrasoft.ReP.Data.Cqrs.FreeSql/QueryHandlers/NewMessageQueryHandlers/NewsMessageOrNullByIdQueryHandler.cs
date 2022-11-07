using Altairis.ReP.Data.Dtos.NewsMessageDtos;
using Altairis.ReP.Data.Queries.NewsMessageQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.NewMessageQueryHandlers;
public class NewsMessageOrNullByIdQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageOrNullByIdQuery, NewsMessageDto?>
{
    public NewsMessageOrNullByIdQueryHandler(IConfigure<NewsMessage> projectionConfigurator, IDataSelector selector)
        : base(projectionConfigurator, selector)
    {}

    protected override async Task<NewsMessageDto?> GetResultToHandleAsync(NewsMessageOrNullByIdQuery query, CancellationToken token)
        => await GetOneOrNullAsync<NewsMessageDto>(nm => nm.Id == query.NewsMessageId,token);
}
