namespace Altairis.ReP.Data.Queries.ReservationQueries;
public class ReservationsByResourceIdAndDateBeginQuery : BaseQuery<IEnumerable<ReservationWithDesignInfoDto>>
{
    public ReservationsByResourceIdAndDateBeginQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResourceId { get; set; }
    public DateTime DateBegin { get; set; }
}
