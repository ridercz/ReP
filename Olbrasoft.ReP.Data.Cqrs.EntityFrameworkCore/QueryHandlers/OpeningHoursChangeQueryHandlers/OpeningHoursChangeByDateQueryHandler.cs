namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangeByDateQueryHandler : RepDbQueryHandler<OpeningHoursChange, OpeningHoursChangeByDateQuery, OpeningHoursChange?>
{
    public OpeningHoursChangeByDateQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<OpeningHoursChange?> GetResultToHandleAsync(OpeningHoursChangeByDateQuery query, CancellationToken token)
        => await GetOneOrNullAsync(ohch => ohch.Date == query.Date, token);
}
