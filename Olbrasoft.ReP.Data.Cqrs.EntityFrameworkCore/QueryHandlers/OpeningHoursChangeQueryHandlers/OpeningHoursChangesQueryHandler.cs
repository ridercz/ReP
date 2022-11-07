namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangesQueryHandler : BaseQueryHandler<OpeningHoursChange, OpeningHoursChangesQuery, IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesQueryHandler(RepDbContext context) : base(context)
    {}

    protected override async Task<IEnumerable<OpeningHoursChange>> GetResultToHandleAsync(OpeningHoursChangesQuery query, CancellationToken token)
        => await OrderByDescending(ohc => ohc.Date).ToArrayAsync(token);
}
