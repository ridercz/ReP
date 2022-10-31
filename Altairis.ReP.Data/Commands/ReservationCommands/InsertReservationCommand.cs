namespace Altairis.ReP.Data.Commands.ReservationCommands;
public class InsertReservationCommand : SaveReservationCommand
{
    public InsertReservationCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResourceId { get; set; }

    public int UserId { get; set; }

}
