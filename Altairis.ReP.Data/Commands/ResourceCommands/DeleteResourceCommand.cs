namespace Altairis.ReP.Data.Commands.ResourceCommands;
public class DeleteResourceCommand : BaseCommand<CommandStatus>
{
    public DeleteResourceCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResourceId { get; set; }
}
