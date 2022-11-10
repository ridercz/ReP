using Altairis.ReP.Data.Dtos.ReservationDtos;
using Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore;
using Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;

namespace Altairis.ReP.Data.Queries.ReservationQueries;
public class ReservationEditQueryHandler : RepDbQueryHandler<Reservation, ReservationEditQuery, ReservationEditDto?>
{
    public ReservationEditQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override Task<ReservationEditDto?> GetResultToHandleAsync(ReservationEditQuery query, CancellationToken token)
        => ProjectTo<ReservationEditDto>(GetWhere(r => r.Id == query.ReservationId)).SingleOrDefaultAsync(token);
}
