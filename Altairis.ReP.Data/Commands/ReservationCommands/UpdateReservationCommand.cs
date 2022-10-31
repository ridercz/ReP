namespace Altairis.ReP.Data.Commands.ReservationCommands;
public class UpdateReservationCommand : SaveReservationCommand
{
    public UpdateReservationCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int Id { get; set; }
}
