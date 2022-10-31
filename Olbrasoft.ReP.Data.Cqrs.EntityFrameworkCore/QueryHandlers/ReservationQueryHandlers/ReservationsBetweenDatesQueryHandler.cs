using Altairis.ReP.Data.Dtos.ReservationDtos;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ReservationQueryHandlers;
public class ReservationsBetweenDatesQueryHandler : BaseQueryHandlerWithProjector<Reservation, ReservationsBetweenDatesQuery, IEnumerable<ReservationWithDesignInfoDto>>
{
    public ReservationsBetweenDatesQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<ReservationWithDesignInfoDto>> GetResultToHandleAsync(ReservationsBetweenDatesQuery query, CancellationToken token) 
        => await ProjectTo<ReservationWithDesignInfoDto>(Where(r => r.DateEnd >= query.DateBegin && r.DateBegin < query.DateEnd))
                               .OrderBy(r => r.DateBegin)
                               .ToArrayAsync(token);
}
