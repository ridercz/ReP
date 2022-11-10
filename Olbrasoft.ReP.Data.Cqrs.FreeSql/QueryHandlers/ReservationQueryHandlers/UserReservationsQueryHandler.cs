namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ReservationQueryHandlers;
public class UserReservationsQueryHandler : RepDbQueryHandler<Reservation, UserReservationQuery, IEnumerable<UserReservationDto>>
{
    public UserReservationsQueryHandler(IConfigure<Reservation> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected override async Task<IEnumerable<UserReservationDto>> GetResultToHandleAsync(UserReservationQuery query, CancellationToken token)
        => await GetEnumerableAsync<UserReservationDto>(GetWhere(r => r.UserId == query.UserId).OrderByDescending(r => r.DateBegin)
        ,token);
}
