namespace Altairis.ReP.Data.Commands.OpenningHoursChangeCommands;
public class DeleteOpeningHoursChangeCommand : BaseCommand<CommandStatus>
{
    public DeleteOpeningHoursChangeCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int OpeningHoursChangeId { get; set; }
}
