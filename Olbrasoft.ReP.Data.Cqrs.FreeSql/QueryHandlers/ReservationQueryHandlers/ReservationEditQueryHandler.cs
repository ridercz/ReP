namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ReservationQueryHandlers;
public class ReservationEditQueryHandler : RepDbQueryHandler<Reservation, ReservationEditQuery, ReservationEditDto?>
{
    public ReservationEditQueryHandler(IConfigure<Reservation> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {
    }

    protected override async Task<ReservationEditDto?> GetResultToHandleAsync(ReservationEditQuery query, CancellationToken token)
        => await GetOneOrNullAsync<ReservationEditDto>(r => r.Id == query.ReservationId, token);
}
