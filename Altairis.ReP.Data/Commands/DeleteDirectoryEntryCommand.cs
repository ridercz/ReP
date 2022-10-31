namespace Altairis.ReP.Data.Commands;
public class DeleteDirectoryEntryCommand : BaseCommand<CommandStatus>
{
    public DeleteDirectoryEntryCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int DirectoryEntryId { get; set; }
}
