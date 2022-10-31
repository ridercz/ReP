namespace Altairis.ReP.Data.Queries.ReservationQueries;
public class ReservationEditQuery : BaseQuery<ReservationEditDto?>
{
    public ReservationEditQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ReservationId { get; set; }
}
