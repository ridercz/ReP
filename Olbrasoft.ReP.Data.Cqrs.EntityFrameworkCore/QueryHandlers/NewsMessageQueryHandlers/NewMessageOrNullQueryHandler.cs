using Altairis.ReP.Data.Queries.NewsMessageQueries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.NewsMessageQueryHandlers;
public class FirstNewMessageOrNullQueryHandler : BaseQueryHandler<NewsMessage, NewsMessageOrNullQuery, NewsMessage?>
{
    public FirstNewMessageOrNullQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<NewsMessage?> GetResultToHandleAsync(NewsMessageOrNullQuery query, CancellationToken token)
        => await OrderByDescending(x => x.Date).FirstOrDefaultAsync(token);
}
