namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.OpeningHoursChangeQueryHandlers;
public class OpeningHoursChangeByDateQueryHandler : RepDbQueryHandler<OpeningHoursChange, OpeningHoursChangeByDateQuery, OpeningHoursChange?>
{
    public OpeningHoursChangeByDateQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }
 
    protected override async Task<OpeningHoursChange?> GetResultToHandleAsync(OpeningHoursChangeByDateQuery query, CancellationToken token)
          => await GetOneOrNullAsync(ohch => ohch.Date == query.Date, token);

}
