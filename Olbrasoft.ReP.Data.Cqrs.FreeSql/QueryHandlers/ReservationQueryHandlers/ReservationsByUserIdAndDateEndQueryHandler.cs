using Altairis.ReP.Data.Dtos.ReservationDtos;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.ReservationQueryHandlers;
public class ReservationsByUserIdAndDateEndQueryHandler : RepDbQueryHandler<Reservation, ReservationsByUserIdAndDateEndQuery, IEnumerable<ReservationInfoDto>>
{
    public ReservationsByUserIdAndDateEndQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    protected override async Task<IEnumerable<ReservationInfoDto>> GetResultToHandleAsync(ReservationsByUserIdAndDateEndQuery query, CancellationToken token)
    {
        var result = await Where(r => r.UserId == query.UserId && r.DateEnd >= query.DateEndToday)
                                        .OrderBy(r => r.DateBegin)
                                        .ToListAsync<ReservationInfoDto>(token);
   
        return result;
    }
}