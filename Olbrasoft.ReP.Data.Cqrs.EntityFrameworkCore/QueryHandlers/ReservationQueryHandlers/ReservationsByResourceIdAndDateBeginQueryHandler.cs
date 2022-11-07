using Altairis.ReP.Data.Dtos.ReservationDtos;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.ReservationQueryHandlers;
public class ReservationsByResourceIdAndDateBeginQueryHandler : BaseQueryHandlerWithProjector<Reservation, ReservationsByResourceIdAndDateBeginQuery, IEnumerable<ReservationWithDesignInfoDto>>
{
    public ReservationsByResourceIdAndDateBeginQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<ReservationWithDesignInfoDto>> GetResultToHandleAsync(ReservationsByResourceIdAndDateBeginQuery query, CancellationToken token) 
        => await ProjectTo<ReservationWithDesignInfoDto>(Where(r => r.ResourceId == query.ResourceId && r.DateBegin >= query.DateBegin))
        .ToArrayAsync(token);
}
