namespace Altairis.ReP.Data.Commands.NewsMessageCommands;
public class DeleteNewsMessageCommand : BaseCommand<CommandStatus>
{
    public DeleteNewsMessageCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int NewsMessageId { get; set; }
}
