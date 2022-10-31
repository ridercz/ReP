namespace Altairis.ReP.Data.Commands;
public class SaveOpeningHoursChangeCommand : BaseCommand<CommandStatus>
{
    public SaveOpeningHoursChangeCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public DateTime Date { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
}
