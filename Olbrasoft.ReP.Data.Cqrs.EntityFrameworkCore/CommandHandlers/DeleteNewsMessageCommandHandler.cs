using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class DeleteNewsMessageCommandHandler : CommandHandler<NewsMessage, DeleteNewsMessageCommand, CommandStatus>
{
    public DeleteNewsMessageCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteNewsMessageCommand command, CancellationToken token)
    {
        if (!await AnyAsync(nm => nm.Id == command.NewsMessageId, token)) return CommandStatus.NotFound;

        return await RemoveAndSaveAsync(new NewsMessage { Id = command.NewsMessageId }, token);
    }
}
