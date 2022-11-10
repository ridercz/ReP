namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.NewMessageQueryHandlers;
public class FirstNewMessageOrNullQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageOrNullQuery, NewsMessage?>
{
    public FirstNewMessageOrNullQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected override async Task<NewsMessage?> GetResultToHandleAsync(NewsMessageOrNullQuery query, CancellationToken token)
        => await GetOneOrNullAsync(GetOrderByDescending(nm => nm.Date), token);
}
