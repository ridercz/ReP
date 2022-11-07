namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;
public class CalendarEntriesBetweenDatesQueryHandler : RepDbQueryHandler<CalendarEntry, CalendarEntriesBetweenDatesQuery, IEnumerable<CalendarEntry>>
{
    public CalendarEntriesBetweenDatesQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    protected override async Task<IEnumerable<CalendarEntry>> GetResultToHandleAsync(CalendarEntriesBetweenDatesQuery query, CancellationToken token)
        => await GetWhere(ce => ce.Date >= query.DateBegin && ce.Date < query.DateEnd)
                                .OrderBy(ce => ce.Date)
                                .ToListAsync(token);
}
