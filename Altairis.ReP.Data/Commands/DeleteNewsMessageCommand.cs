namespace Altairis.ReP.Data.Commands;
public class DeleteNewsMessageCommand : BaseCommand<CommandStatus>
{
    public DeleteNewsMessageCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int NewsMessageId { get; set; }
}
