using Altairis.ReP.Data.Dtos.ReservationDtos;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ReservationQueryHandlers;
public class ReservationFullInfosQueryHandler : BaseQueryHandlerWithProjector<Reservation, ReservationFullInfosQuery, IEnumerable<ReservationFullInfoDto>>
{
    public ReservationFullInfosQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<ReservationFullInfoDto>> GetResultToHandleAsync(ReservationFullInfosQuery query, CancellationToken token) 
        => await ProjectTo<ReservationFullInfoDto>(OrderByDescending(r => r.DateBegin)).ToArrayAsync(token);
}
