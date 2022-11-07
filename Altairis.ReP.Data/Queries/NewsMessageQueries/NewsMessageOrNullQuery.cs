using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries.NewsMessageQueries;
public class NewsMessageOrNullQuery : BaseQuery<NewsMessage?>
{
    public NewsMessageOrNullQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
