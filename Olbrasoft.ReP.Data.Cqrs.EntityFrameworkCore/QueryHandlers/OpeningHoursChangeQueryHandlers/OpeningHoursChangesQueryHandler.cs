namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangesQueryHandler : RepDbQueryHandler<OpeningHoursChange, OpeningHoursChangesQuery, IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesQueryHandler(RepDbContext context) : base(context)
    {}

    protected override async Task<IEnumerable<OpeningHoursChange>> GetResultToHandleAsync(OpeningHoursChangesQuery query, CancellationToken token)
        => await GetOrderByDescending(ohc => ohc.Date).ToArrayAsync(token);
}
