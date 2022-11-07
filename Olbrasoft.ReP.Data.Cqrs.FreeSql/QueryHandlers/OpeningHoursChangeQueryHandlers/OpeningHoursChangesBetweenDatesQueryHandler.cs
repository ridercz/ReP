using Altairis.ReP.Data.Queries.OpeningHoursChangeQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangesBetweenDatesQueryHandler :
    RepDbQueryHandler<OpeningHoursChange, OpeningHoursChangesBetweenDatesQuery, IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesBetweenDatesQueryHandler(IConfigure<OpeningHoursChange> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {
    }

    protected override async Task<IEnumerable<OpeningHoursChange>> GetResultToHandleAsync(OpeningHoursChangesBetweenDatesQuery query, CancellationToken token)
        => await GetEnumerableAsync(GetWhere(x => x.Date >= query.DateFrom && x.Date <= query.DateTo), token);
}
