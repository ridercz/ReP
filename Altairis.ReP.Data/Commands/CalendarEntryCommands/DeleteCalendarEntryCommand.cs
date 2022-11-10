namespace Altairis.ReP.Data.Commands.CalendarEntryCommands;
public class DeleteCalendarEntryCommand : BaseCommand<CommandStatus>
{
    public DeleteCalendarEntryCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int Id { get; set; }
}
