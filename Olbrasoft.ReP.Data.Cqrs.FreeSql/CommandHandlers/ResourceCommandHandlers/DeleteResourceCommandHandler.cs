using Altairis.ReP.Data.Commands.ResourceCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.ResourceCommandHandlers;
public class DeleteResourceCommandHandler : RepDbCommandHandler<Resource, DeleteResourceCommand, CommandStatus>
{
    public DeleteResourceCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteResourceCommand command, CancellationToken token)
        => await ExistsAsync(r => r.Id == command.ResourceId, token)
           ? await RemoveAndSaveAsync(new Resource { Id = command.ResourceId }, token)
           : CommandStatus.NotFound;
}
