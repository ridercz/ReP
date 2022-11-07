using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries.DirectoryEntryQueries;

namespace Olbrasoft.ReP.Business;
public class DirectoryEntryService : BaseService, IDirectoryEntryService
{
    public DirectoryEntryService(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public Task<CommandStatus> DeleteAsync(int Id, CancellationToken token = default) 
        => new DeleteDirectoryEntryCommand(Dispatcher) { DirectoryEntryId = Id }.ToResultAsync(token);

    public async Task<IEnumerable<DirectoryEntry>> GetDirectoryEntriesAsync(CancellationToken token = default)
        => await new DirectoryEntriesQuery(Dispatcher).ToResultAsync(token);

    public async Task<DirectoryEntry?> GetDirectoryEntryOrNullAsync(int Id, CancellationToken token = default)
        => await new DirectoryEntryQuery(Dispatcher) { DirectoryEntryId = Id }.ToResultAsync(token);


    public async Task<IEnumerable<DirectoryEntryInfoDto>> GetDirectoryInfosAsync(bool ShouldBeUsers = false, CancellationToken token = default) 
        => ShouldBeUsers
            ? await new DirectoryEntryInfosWhereShowInMemberDirectoryQuery(Dispatcher).ToResultAsync(token)
            : await new DirectoryEntryInfosQuery(Dispatcher).ToResultAsync(token);

    public async Task<CommandStatus> SaveAsync(int Id, string displayName, string? email, string? phoneNumber, CancellationToken token = default)
        => await new SaveDirectoryEntryCommand(Dispatcher)
        {
            Id = Id,
            DisplayName = displayName,
            Email = email,
            PhoneNumber = phoneNumber
        }.ToResultAsync(token);

    async Task IDirectoryEntryService.SaveAsync(string displayName, string? email, string? phoneNumber, CancellationToken token)
        => await new SaveDirectoryEntryCommand(Dispatcher)
        {
            DisplayName = displayName,
            Email = email,
            PhoneNumber = phoneNumber
        }.ToResultAsync(token);
}