using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries.OpeningHoursChangeQueries;
public class OpeningHoursChangesBetweenDatesQuery : BaseQuery<IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesBetweenDatesQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
