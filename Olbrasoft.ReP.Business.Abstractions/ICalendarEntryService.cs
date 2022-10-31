using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface ICalendarEntryService
{
    Task<IEnumerable<CalendarEntry>> GetCalendarEntriesBetweenAsync(DateTime dateBegin, DateTime dateEnd, CancellationToken token = default);
    Task<CommandStatus> DeleteCalendarEntryByIdAsync(int id, CancellationToken token = default);
    Task<CommandStatus> CreateCalendarEntryAsync(DateTime date, string title, string? comment, CancellationToken token = default);
}
