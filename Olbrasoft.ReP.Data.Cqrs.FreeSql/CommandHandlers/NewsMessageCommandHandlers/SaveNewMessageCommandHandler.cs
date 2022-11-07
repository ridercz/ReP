namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.NewsMessageCommandHandlers;
public class SaveNewMessageCommandHandler : RepDbCommandHandler<NewsMessage, SaveNewMessageCommand, CommandStatus>
{
    public SaveNewMessageCommandHandler(IMapper mapper, IDbContextProxy proxy) : base(mapper, proxy)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveNewMessageCommand command, CancellationToken token)
    {
        if (command.Id <= 0) return await AddAndSaveAsync(MapCommandToNewEntity(command), token);
               
        var newsMessage = await GetOneOrNullAsync(nm => nm.Id == command.Id, token);

        if (newsMessage is null) return CommandStatus.NotFound;

        newsMessage.Text = command.Text;
        newsMessage.Title = command.Title;
        
        return await UpdateAndSaveAsync(newsMessage, token);
    }
}
