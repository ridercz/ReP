namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ReservationQueryHandlers;
public class ReservationFullInfosQueryHandler : RepDbQueryHandler<Reservation, ReservationFullInfosQuery, IEnumerable<ReservationFullInfoDto>>
{
    public ReservationFullInfosQueryHandler(IConfigure<Reservation> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }


    protected override async Task<IEnumerable<ReservationFullInfoDto>> GetResultToHandleAsync(ReservationFullInfosQuery query, CancellationToken token)
        => await GetOrderByDescending(r => r.DateBegin)
        .ToListAsync(r =>  new ReservationFullInfoDto { UserDisplayName = r.User!.DisplayName, ResourceName = r.Resource!.Name } , token);
}
