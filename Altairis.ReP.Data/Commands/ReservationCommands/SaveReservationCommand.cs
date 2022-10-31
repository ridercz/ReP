namespace Altairis.ReP.Data.Commands.ReservationCommands;
public abstract class SaveReservationCommand : BaseCommand<SaveReservationCommandResult>
{
    protected SaveReservationCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public bool System { get; set; } = false;

    public string? Comment { get; set; }
}
