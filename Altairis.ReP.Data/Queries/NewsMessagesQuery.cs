using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries;
public class NewsMessagesQuery : BaseQuery<IEnumerable<NewsMessage>>
{
    public NewsMessagesQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
