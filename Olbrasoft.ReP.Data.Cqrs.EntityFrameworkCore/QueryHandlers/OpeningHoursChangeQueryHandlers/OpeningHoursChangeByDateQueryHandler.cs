using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries.OpeningHoursChangeQueries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangeByDateQueryHandler : BaseQueryHandler<OpeningHoursChange, OpeningHoursChangeByDateQuery, OpeningHoursChange?>
{
    public OpeningHoursChangeByDateQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<OpeningHoursChange?> GetResultToHandleAsync(OpeningHoursChangeByDateQuery query, CancellationToken token)
        => await SingleOrDefaultAsync(ohch => ohch.Date == query.Date, token);
}
