namespace Altairis.ReP.Data.Queries.ReservationQueries;
public class ReservationFullInfosQuery : BaseQuery<IEnumerable<ReservationFullInfoDto>>
{
    public ReservationFullInfosQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
