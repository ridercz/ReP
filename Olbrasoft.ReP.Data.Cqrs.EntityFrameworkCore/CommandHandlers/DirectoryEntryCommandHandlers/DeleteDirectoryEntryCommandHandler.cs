using Altairis.ReP.Data.Commands.DirectoryEntryCommands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.DirectoryEntryCommandHandlers;
public class DeleteDirectoryEntryCommandHandler : RepDbCommandHandler<DirectoryEntry, DeleteDirectoryEntryCommand, CommandStatus>
{
    public DeleteDirectoryEntryCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteDirectoryEntryCommand command, CancellationToken token) 
        => await RemoveAndSaveAsync(de => de.Id == command.DirectoryEntryId, token);
}
