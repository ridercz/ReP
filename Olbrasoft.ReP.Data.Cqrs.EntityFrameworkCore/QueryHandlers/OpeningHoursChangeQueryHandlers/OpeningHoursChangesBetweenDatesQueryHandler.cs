namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangesBetweenDatesQueryHandler :
    BaseQueryHandler<OpeningHoursChange, OpeningHoursChangesBetweenDatesQuery, IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesBetweenDatesQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<IEnumerable<OpeningHoursChange>> GetResultToHandleAsync(OpeningHoursChangesBetweenDatesQuery query, CancellationToken token)
        => await Where(x => x.Date >= query.DateFrom && x.Date <= query.DateTo).ToArrayAsync(token);
}
