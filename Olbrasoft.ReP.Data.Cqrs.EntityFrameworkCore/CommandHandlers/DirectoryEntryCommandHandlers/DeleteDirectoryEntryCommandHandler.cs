using Altairis.ReP.Data.Commands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.DirectoryEntryCommandHandlers;
public class DeleteDirectoryEntryCommandHandler : DbCommandHandler<DirectoryEntry, DeleteDirectoryEntryCommand, CommandStatus>
{
    public DeleteDirectoryEntryCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteDirectoryEntryCommand command, CancellationToken token)
    {
        var direcoryEntry = await GetOneOrNullAsync(de => de.Id == command.DirectoryEntryId, token);

        return direcoryEntry is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(direcoryEntry, token);
    }
}
