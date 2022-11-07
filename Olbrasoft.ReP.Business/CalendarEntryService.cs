using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business;
public class CalendarEntryService : BaseService, ICalendarEntryService
{
    public CalendarEntryService(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public async Task<CommandStatus> SaveAsync(DateTime date, string title, string? comment, CancellationToken token = default)
    {
        var status = await new SaveCalendarEntryCommand(Dispatcher) { Date = date, Title = title, Comment = comment }.ToResultAsync(token);

        ThrowExceptionIfStatusIsError(status);

        return status;
    }

    public async Task<CommandStatus> DeleteCalendarEntryByIdAsync(int id, CancellationToken token = default)
    {
        var status = await new DeleteCalendarEntryCommand(Dispatcher) { Id = id }.ToResultAsync(token: token);

        ThrowExceptionIfStatusIsError(status);

        return status;
    }

    public async Task<IEnumerable<CalendarEntry>> GetBetweenDatesAsync(DateTime dateBegin,
                                                                                 DateTime dateEnd,
                                                                                 CancellationToken token = default)
        => await new CalendarEntriesBetweenDatesQuery(Dispatcher)
        {
            DateBegin = dateBegin,
            DateEnd = dateEnd

        }.ToResultAsync(token);
}
