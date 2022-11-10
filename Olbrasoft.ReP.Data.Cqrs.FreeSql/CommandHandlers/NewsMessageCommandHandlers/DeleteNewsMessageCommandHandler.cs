using Altairis.ReP.Data.Commands.NewsMessageCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.NewsMessageCommandHandlers;
public class DeleteNewsMessageCommandHandler : RepDbCommandHandler<NewsMessage, DeleteNewsMessageCommand, CommandStatus>
{
    public DeleteNewsMessageCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteNewsMessageCommand command, CancellationToken token)
    {
        if (!await ExistsAsync(nm => nm.Id == command.NewsMessageId, token)) return CommandStatus.NotFound;

        return await RemoveAndSaveAsync(new NewsMessage { Id = command.NewsMessageId }, token);
    }
}
