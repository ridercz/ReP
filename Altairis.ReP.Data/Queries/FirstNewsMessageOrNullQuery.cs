using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries;
public class FirstNewsMessageOrNullQuery : BaseQuery<NewsMessage?>
{
    public FirstNewsMessageOrNullQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
