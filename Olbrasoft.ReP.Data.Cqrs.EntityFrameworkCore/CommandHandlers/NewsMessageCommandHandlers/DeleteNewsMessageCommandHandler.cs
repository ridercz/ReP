using Altairis.ReP.Data.Commands.NewsMessageCommands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.NewsMessageCommandHandlers;
public class DeleteNewsMessageCommandHandler : RepDbCommandHandler<NewsMessage, DeleteNewsMessageCommand, CommandStatus>
{
    public DeleteNewsMessageCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteNewsMessageCommand command, CancellationToken token) 
        => !await ExistsAsync(nm => nm.Id == command.NewsMessageId, token)
            ? CommandStatus.NotFound
            : await RemoveAndSaveAsync(new NewsMessage { Id = command.NewsMessageId }, token);
}
