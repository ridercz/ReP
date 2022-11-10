using Altairis.ReP.Data.Commands.OpenningHoursChangeCommands;
using Altairis.ReP.Data.Queries.OpeningHoursChangeQueries;

namespace Olbrasoft.ReP.Business;
public class OpeningHoursChangeService : BaseService, IOpeningHoursChangeService
{
    public OpeningHoursChangeService(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public Task<CommandStatus> DeleteOpeningHoursChangeAsync(int openingHoursChangeId, CancellationToken token = default)
        => new DeleteOpeningHoursChangeCommand(Dispatcher) { OpeningHoursChangeId = openingHoursChangeId }.ToResultAsync(token);

    public async Task<OpeningHoursChange?> GetOpeningHoursChangeOrNullByAsync(DateTime date, CancellationToken token = default)
        => await new OpeningHoursChangeByDateQuery(Dispatcher) { Date = date }.ToResultAsync(token);


    public async Task<IEnumerable<OpeningHoursChange>> GetOpeningHoursChangesAsync(CancellationToken token = default)
        => await new OpeningHoursChangesQuery(Dispatcher).ToResultAsync(token);

    public async Task<IEnumerable<OpeningHoursChange>> GetOpeningHoursChangesBetween(DateTime dateFrom, DateTime dateTo, CancellationToken token = default)
        => await new OpeningHoursChangesBetweenDatesQuery(Dispatcher) { DateFrom = dateFrom, DateTo = dateTo }.ToResultAsync(token);

    public async Task<CommandStatus> SaveOpeningHoursChangeAsync(DateTime date, TimeSpan openingTime, TimeSpan closingTime, CancellationToken token = default)
        => await new SaveOpeningHoursChangeCommand(Dispatcher) { Date = date, OpeningTime = openingTime, ClosingTime = closingTime }.ToResultAsync(token);
}
