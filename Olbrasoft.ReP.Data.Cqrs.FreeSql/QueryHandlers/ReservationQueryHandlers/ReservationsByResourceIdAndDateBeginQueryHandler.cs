namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ReservationQueryHandlers;

public class ReservationsByResourceIdAndDateBeginQueryHandler
    : RepDbQueryHandler<Reservation, ReservationsByResourceIdAndDateBeginQuery, IEnumerable<ReservationWithDesignInfoDto>>
{
    public ReservationsByResourceIdAndDateBeginQueryHandler(IConfigure<Reservation> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<ReservationWithDesignInfoDto>> GetResultToHandleAsync(ReservationsByResourceIdAndDateBeginQuery query,
                                                                                                   CancellationToken token) 
    => await GetEnumerableAsync<ReservationWithDesignInfoDto>(r => r.ResourceId == query.ResourceId && r.DateBegin >= query.DateBegin, token);
}