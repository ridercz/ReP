using Altairis.ReP.Data.Commands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.NewsMessageCommandHandlers;
public class DeleteNewsMessageCommandHandler : DbCommandHandler<NewsMessage, DeleteNewsMessageCommand, CommandStatus>
{
    public DeleteNewsMessageCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteNewsMessageCommand command, CancellationToken token)
    {
        if (!await ExistsAsync(nm => nm.Id == command.NewsMessageId, token)) return CommandStatus.NotFound;

        return await RemoveAndSaveAsync(new NewsMessage { Id = command.NewsMessageId }, token);
    }
}
