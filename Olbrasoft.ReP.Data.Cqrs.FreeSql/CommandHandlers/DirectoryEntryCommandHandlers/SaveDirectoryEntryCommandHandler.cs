using Altairis.ReP.Data.Commands.DirectoryEntryCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.DirectoryEntryCommandHandlers;
public class SaveDirectoryEntryCommandHandler : RepDbCommandHandler<DirectoryEntry, SaveDirectoryEntryCommand, CommandStatus>
{
    public SaveDirectoryEntryCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveDirectoryEntryCommand command, CancellationToken token)
    {
        if (command.Id <= 0) return await AddAndSaveAsync(MapCommandToNewEntity(command), token);

        var directoryEntry = await GetOneOrNullAsync(de => de.Id == command.Id, token);

        return directoryEntry is null ? CommandStatus.NotFound : await SaveAsync(MapCommandToExistingEntity(command, directoryEntry), token);
    }
}
