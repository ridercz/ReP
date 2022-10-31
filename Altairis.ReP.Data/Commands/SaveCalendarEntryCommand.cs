namespace Altairis.ReP.Data.Commands;
public class SaveCalendarEntryCommand : BaseCommand<CommandStatus>
{
    public SaveCalendarEntryCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Comment { get; set; }
}
