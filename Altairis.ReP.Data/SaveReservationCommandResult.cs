namespace Altairis.ReP.Data;
public class SaveReservationCommandResult
{
    public CommandStatus Status { get; set; }

    public IEnumerable<ReservationConflictDto> Conflicts { get; set; } = Enumerable.Empty<ReservationConflictDto>();
}
