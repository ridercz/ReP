using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries.OpeningHoursChangeQueries;
public class OpeningHoursChangesQuery : BaseQuery<IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
