namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ReservationQueryHandlers;

public class ReservationsByResourceIdAndDateBeginQueryHandler
    : RepDbQueryHandler<Reservation, ReservationsByResourceIdAndDateBeginQuery, IEnumerable<ReservationWithDesignInfoDto>>
{
    public ReservationsByResourceIdAndDateBeginQueryHandler(IConfigure<Reservation> projectionConfigurator, IDataSelector selector)
        : base(projectionConfigurator, selector)
    {}

    protected override async Task<IEnumerable<ReservationWithDesignInfoDto>> GetResultToHandleAsync(ReservationsByResourceIdAndDateBeginQuery query,
                                                                                                   CancellationToken token) 
        => await GetEnumerableAsync<ReservationWithDesignInfoDto>(
            GetWhere(r => r.ResourceId == query.ResourceId && r.DateBegin >= query.DateBegin), token);
}