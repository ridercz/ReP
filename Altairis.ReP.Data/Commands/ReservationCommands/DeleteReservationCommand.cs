namespace Altairis.ReP.Data.Commands.ReservationCommands;
public class DeleteReservationCommand : BaseCommand<CommandStatus>
{
    public DeleteReservationCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResevationId { get; set; }
    public int UserId { get; set; }
    public DateTime Now { get; set; }
}
