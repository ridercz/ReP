using Altairis.ReP.Data.Queries.OpeningHoursChangeQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangeByDateQueryHandler : DbQueryHandler<OpeningHoursChange, OpeningHoursChangeByDateQuery, OpeningHoursChange?>
{
    public OpeningHoursChangeByDateQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    public override async Task<OpeningHoursChange?> HandleAsync(OpeningHoursChangeByDateQuery query, CancellationToken token)
        => await Select.Where(ohch => ohch.Date == query.Date).ToOneAsync(token);
}
