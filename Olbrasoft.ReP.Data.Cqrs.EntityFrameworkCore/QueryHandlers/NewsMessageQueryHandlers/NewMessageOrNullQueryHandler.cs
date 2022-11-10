namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.NewsMessageQueryHandlers;
public class FirstNewMessageOrNullQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageOrNullQuery, NewsMessage?>
{
    public FirstNewMessageOrNullQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<NewsMessage?> GetResultToHandleAsync(NewsMessageOrNullQuery query, CancellationToken token)
        => await GetOrderByDescending(x => x.Date).FirstOrDefaultAsync(token);
}
