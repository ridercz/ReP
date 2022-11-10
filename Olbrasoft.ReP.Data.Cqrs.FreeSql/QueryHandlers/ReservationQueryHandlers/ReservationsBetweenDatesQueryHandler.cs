namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ReservationQueryHandlers;
public class ReservationsBetweenDatesQueryHandler : RepDbQueryHandler<Reservation, ReservationsBetweenDatesQuery, IEnumerable<ReservationWithDesignInfoDto>>
{
    public ReservationsBetweenDatesQueryHandler(IConfigure<Reservation> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<ReservationWithDesignInfoDto>> GetResultToHandleAsync(ReservationsBetweenDatesQuery query, CancellationToken token)
    {
        return await GetEnumerableAsync<ReservationWithDesignInfoDto>(GetWhere(r => r.DateEnd >= query.DateBegin && r.DateBegin < query.DateEnd)
                                  .OrderBy(r => r.DateBegin),token);
    }
}
