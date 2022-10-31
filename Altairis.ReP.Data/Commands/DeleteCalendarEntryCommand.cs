namespace Altairis.ReP.Data.Commands;
public class DeleteCalendarEntryCommand : BaseCommand<CommandStatus>
{
    public DeleteCalendarEntryCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int Id { get; set; }
}
