using Altairis.ReP.Data.Dtos.ReservationDtos;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ReservationQueryHandlers;
public class UserReservationsQueryHandler : BaseQueryHandlerWithProjector<Reservation, UserReservationQuery, IEnumerable<UserReservationDto>>
{
    public UserReservationsQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<UserReservationDto>> GetResultToHandleAsync(UserReservationQuery query, CancellationToken token) 
        => await ProjectTo<UserReservationDto>(Where(r => r.UserId == query.UserId)
                                                   .OrderByDescending(r => r.DateBegin)).ToArrayAsync(token);
}
