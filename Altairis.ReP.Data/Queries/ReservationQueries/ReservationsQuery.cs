namespace Altairis.ReP.Data.Queries.ReservationQueries;
public class ReservationsByUserIdAndDateEndQuery : BaseQuery<IEnumerable<ReservationInfoDto>>
{
    public ReservationsByUserIdAndDateEndQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int UserId { get; set; }
    public DateTime DateEndToday { get; set; }
    public DateTime Now { get; set; }

}
