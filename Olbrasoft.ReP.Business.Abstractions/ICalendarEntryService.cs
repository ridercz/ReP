using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface ICalendarEntryService
{
    Task<IEnumerable<CalendarEntry>> GetBetweenDatesAsync(DateTime dateBegin, DateTime dateEnd, CancellationToken token = default);
    Task<CommandStatus> DeleteCalendarEntryByIdAsync(int id, CancellationToken token = default);
    Task<CommandStatus> SaveAsync(DateTime date, string title, string? comment, CancellationToken token = default);
}
