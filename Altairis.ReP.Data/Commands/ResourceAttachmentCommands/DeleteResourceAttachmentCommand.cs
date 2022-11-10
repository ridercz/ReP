namespace Altairis.ReP.Data.Commands.ResourceAttachmentCommands;
public class DeleteResourceAttachmentCommand : BaseCommand<CommandStatus>
{
    public DeleteResourceAttachmentCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResourceAttachmentId { get; set; }

}
