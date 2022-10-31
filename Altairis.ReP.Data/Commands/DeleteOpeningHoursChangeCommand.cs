namespace Altairis.ReP.Data.Commands;
public class DeleteOpeningHoursChangeCommand : BaseCommand<CommandStatus>
{
    public DeleteOpeningHoursChangeCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int OpeningHoursChangeId { get; set; }
}
