using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries.OpeningHoursChangeQueries;
public class OpeningHoursChangeByDateQuery : BaseQuery<OpeningHoursChange?>
{
    public OpeningHoursChangeByDateQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public DateTime Date { get; set; }
}
