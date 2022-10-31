namespace Altairis.ReP.Data.Commands;
public class DeleteResourceAttachmentCommand : BaseCommand<CommandStatus>
{
    public DeleteResourceAttachmentCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResourceAttachmentId { get; set; }

}
