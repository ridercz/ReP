namespace Altairis.ReP.Data.Queries.ReservationQueries;
public class ReservationsBetweenDatesQuery : BaseQuery<IEnumerable<ReservationWithDesignInfoDto>>
{
    public ReservationsBetweenDatesQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
}
