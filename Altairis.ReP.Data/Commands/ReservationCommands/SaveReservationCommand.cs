namespace Altairis.ReP.Data.Commands.ReservationCommands;
public class SaveReservationCommand : BaseCommand<SaveReservationCommandResult>
{
    public SaveReservationCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int Id { get; set; }

    public int ResourceId { get; set; }

    public int UserId { get; set; }

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public bool System { get; set; } = false;

    public string? Comment { get; set; }
}
