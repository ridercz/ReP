namespace Altairis.ReP.Data.Commands.DirectoryEntryCommands;

public class SaveDirectoryEntryCommand : BaseCommand<CommandStatus>
{
    public SaveDirectoryEntryCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

}
