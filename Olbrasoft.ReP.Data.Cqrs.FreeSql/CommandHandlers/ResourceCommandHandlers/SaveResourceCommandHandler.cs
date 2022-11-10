using Altairis.ReP.Data.Commands.ResourceCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.ResourceCommandHandlers;
public class SaveResourceCommandHandler : RepDbCommandHandler<Resource, SaveResourceCommand, CommandStatus>
{
    public SaveResourceCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {}

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveResourceCommand command, CancellationToken token) 
        => command.Id <= 0
            ? await AddAndSaveAsync(MapCommandToNewEntity(command), token)
            : await ExistsAsync(r => r.Id == command.Id, token)
            ? await UpdateAndSaveAsync(MapCommandToNewEntity(command), token)
            : CommandStatus.NotFound;
}
