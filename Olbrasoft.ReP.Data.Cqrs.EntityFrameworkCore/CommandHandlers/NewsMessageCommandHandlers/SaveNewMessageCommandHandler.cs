using Altairis.ReP.Data.Commands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.NewsMessageCommandHandlers;
public class SaveNewMessageCommandHandler : CommandHandlerWithMapper<NewsMessage, SaveNewMessageCommand, CommandStatus>
{
    public SaveNewMessageCommandHandler(IMapper mapper, RepDbContext context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveNewMessageCommand command, CancellationToken token)
    {
        if (command.Id <= 0) return await AddAndSaveAsync(MapCommandToNewEntity(command), token);

        var newsMessage = await GetOneOrNullAsync(nm => nm.Id == command.Id, token);

        if (newsMessage is null) return CommandStatus.NotFound;

        newsMessage.Text = command.Text;
        newsMessage.Title = command.Title;

        return await SaveAsync(newsMessage, token);
    }
}
