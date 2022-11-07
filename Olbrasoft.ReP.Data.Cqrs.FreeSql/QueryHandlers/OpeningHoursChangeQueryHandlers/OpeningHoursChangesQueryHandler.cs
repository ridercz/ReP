using Altairis.ReP.Data.Queries.OpeningHoursChangeQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangesQueryHandler : RepDbQueryHandler<OpeningHoursChange, OpeningHoursChangesQuery, IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesQueryHandler(IDataSelector selector) : base(selector)
    {
    }
    protected override async Task<IEnumerable<OpeningHoursChange>> GetResultToHandleAsync(OpeningHoursChangesQuery query, CancellationToken token)
        => await GetOrderByDescending(ohc => ohc.Date).ToListAsync(token);
}
