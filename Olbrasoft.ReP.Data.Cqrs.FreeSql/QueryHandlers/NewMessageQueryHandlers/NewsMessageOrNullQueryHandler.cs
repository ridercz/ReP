using Altairis.ReP.Data.Queries.NewsMessageQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.NewMessageQueryHandlers;
public class FirstNewMessageOrNullQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageOrNullQuery, NewsMessage?>
{
    public FirstNewMessageOrNullQueryHandler(IDataSelector selector) : base(selector)
    {}

    protected override async Task<NewsMessage?> GetResultToHandleAsync(NewsMessageOrNullQuery query, CancellationToken token)
        => await GetOneOrNullAsync(GetOrderByDescending(nm => nm.Date), token);
}
