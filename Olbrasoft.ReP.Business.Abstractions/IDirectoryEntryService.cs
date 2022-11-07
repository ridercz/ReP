using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface IDirectoryEntryService
{
    Task<IEnumerable<DirectoryEntry>> GetDirectoryEntriesAsync(CancellationToken token = default);
    Task<DirectoryEntry?> GetDirectoryEntryOrNullAsync(int Id, CancellationToken token = default);
    Task<CommandStatus> DeleteAsync(int Id, CancellationToken token = default);
    Task SaveAsync(string displayName, string? email, string? phoneNumber, CancellationToken token = default);
    Task<CommandStatus> SaveAsync(int Id, string displayName, string? email, string? phoneNumber, CancellationToken token = default);
    Task<IEnumerable<DirectoryEntryInfoDto>>GetDirectoryInfosAsync(bool ShouldBeUsers = false, CancellationToken token = default);
}