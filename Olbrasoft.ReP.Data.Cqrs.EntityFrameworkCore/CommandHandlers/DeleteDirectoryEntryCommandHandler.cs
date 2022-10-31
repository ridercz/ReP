using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class DeleteDirectoryEntryCommandHandler : CommandHandler<DirectoryEntry, DeleteDirectoryEntryCommand, CommandStatus>
{
    public DeleteDirectoryEntryCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteDirectoryEntryCommand command, CancellationToken token)
    {
        var direcoryEntry = await SingleOrDefaultAsync(de => de.Id == command.DirectoryEntryId, token);

        return direcoryEntry is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(direcoryEntry, token);
    }
}
