namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangesQueryHandler : RepDbQueryHandler<OpeningHoursChange, OpeningHoursChangesQuery, IEnumerable<OpeningHoursChange>>
{
    public OpeningHoursChangesQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected override async Task<IEnumerable<OpeningHoursChange>> GetResultToHandleAsync(OpeningHoursChangesQuery query, CancellationToken token)
        => await GetOrderByDescending(ohc => ohc.Date).ToListAsync(token);
}
