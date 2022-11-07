namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ReservationQueryHandlers;
public class ReservationsByUserIdAndDateEndQueryHandler : RepDbQueryHandler<Reservation, ReservationsByUserIdAndDateEndQuery, IEnumerable<ReservationInfoDto>>
{
    public ReservationsByUserIdAndDateEndQueryHandler(IConfigure<Reservation> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {
    }

    protected override async Task<IEnumerable<ReservationInfoDto>> GetResultToHandleAsync(ReservationsByUserIdAndDateEndQuery query, CancellationToken token) 
        => await GetEnumerableAsync<ReservationInfoDto>(GetWhere(r => r.UserId == query.UserId && r.DateEnd >= query.DateEndToday)
                                            .OrderBy(r => r.DateBegin)
                                            ,token);
}