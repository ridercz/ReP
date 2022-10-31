using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface IOpeningHoursChangeService
{
    Task<IEnumerable<OpeningHoursChange>> GetOpeningHoursChangesAsync(CancellationToken token = default);

    Task<CommandStatus> SaveOpeningHoursChangeAsync(DateTime date, TimeSpan openingTime, TimeSpan closingTime, CancellationToken token = default);

    Task<CommandStatus> DeleteOpeningHoursChangeAsync(int openingHoursChangeId, CancellationToken token = default);

    Task<OpeningHoursChange?> GetOpeningHoursChangeOrNullByAsync(DateTime date, CancellationToken token = default);

    Task<IEnumerable<OpeningHoursChange>> GetOpeningHoursChangesBetween(DateTime dateFrom, DateTime dateTo, CancellationToken token = default);
}
