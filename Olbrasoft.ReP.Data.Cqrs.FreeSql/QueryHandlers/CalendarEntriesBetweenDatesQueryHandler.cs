namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;
public class CalendarEntriesBetweenDatesQueryHandler : RepDbQueryHandler<CalendarEntry, CalendarEntriesBetweenDatesQuery, IEnumerable<CalendarEntry>>
{
    public CalendarEntriesBetweenDatesQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected override async Task<IEnumerable<CalendarEntry>> GetResultToHandleAsync(CalendarEntriesBetweenDatesQuery query, CancellationToken token)
        => await GetEnumerableAsync(
            GetWhere(ce => ce.Date >= query.DateBegin && ce.Date < query.DateEnd).OrderBy(ce => ce.Date), token);
                                
}
