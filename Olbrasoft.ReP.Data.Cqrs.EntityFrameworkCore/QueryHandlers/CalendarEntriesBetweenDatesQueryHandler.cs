namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class CalendarEntriesBetweenDatesQueryHandler : RepDbQueryHandler<CalendarEntry, CalendarEntriesBetweenDatesQuery, IEnumerable<CalendarEntry>>
{
    public CalendarEntriesBetweenDatesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<CalendarEntry>> GetResultToHandleAsync(CalendarEntriesBetweenDatesQuery query, CancellationToken token) 
        => await GetWhere(ce => ce.Date >= query.DateBegin && ce.Date < query.DateEnd)
                                .OrderBy(ce => ce.Date)
                                .ToArrayAsync(token);
}
