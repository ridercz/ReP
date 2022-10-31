using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Queries;
public class CalendarEntriesBetweenDatesQuery : BaseQuery<IEnumerable<CalendarEntry>>
{
    public CalendarEntriesBetweenDatesQuery(IDispatcher dispatcher) : base(dispatcher)
    {
              
    }

    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
}
