using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries.NewsMessageQueries;
public class NewsMessagesQuery : BaseQuery<IEnumerable<NewsMessage>>
{
    public NewsMessagesQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
