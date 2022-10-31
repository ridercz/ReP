using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;
public class FirstNewMessageOrNullQueryHandler : RepDbQueryHandler<NewsMessage, FirstNewsMessageOrNullQuery, NewsMessage?>
{
    public FirstNewMessageOrNullQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    protected override async Task<NewsMessage?> GetResultToHandleAsync(FirstNewsMessageOrNullQuery query, CancellationToken token)
        => await OrderByDescending(nm => nm.Date).ToOneAsync(token);
}
