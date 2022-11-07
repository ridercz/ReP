using Altairis.ReP.Data.Dtos.ReservationDtos;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ReservationQueryHandlers;
public class ReservationsByUserIdAndDateEndQueryHandler : BaseQueryHandlerWithProjector<Reservation, ReservationsByUserIdAndDateEndQuery, IEnumerable<ReservationInfoDto>>
{
    public ReservationsByUserIdAndDateEndQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<ReservationInfoDto>> GetResultToHandleAsync(ReservationsByUserIdAndDateEndQuery query, CancellationToken token) 
        => await ProjectTo<ReservationInfoDto>(Where(r => r.UserId == query.UserId && r.DateEnd >= query.DateEndToday)
                                        .OrderBy(r => r.DateBegin)).ToArrayAsync(token);
}