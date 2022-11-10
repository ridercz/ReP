namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangesBetweenDatesQueryHandler :
    RepDbQueryHandler<OpeningHoursChange, OpeningHoursChangesBetweenDatesQuery, IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesBetweenDatesQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected override async Task<IEnumerable<OpeningHoursChange>> GetResultToHandleAsync(OpeningHoursChangesBetweenDatesQuery query, CancellationToken token)
        => await GetEnumerableAsync(x => x.Date >= query.DateFrom && x.Date <= query.DateTo, token);
}
