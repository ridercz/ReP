using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class FirstNewMessageOrNullQueryHandler : BaseQueryHandler<NewsMessage, FirstNewsMessageOrNullQuery, NewsMessage?>
{
    public FirstNewMessageOrNullQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<NewsMessage?> GetResultToHandleAsync(FirstNewsMessageOrNullQuery query, CancellationToken token) 
        => await OrderByDescending(x => x.Date).FirstOrDefaultAsync(token);
}
