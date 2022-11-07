using Altairis.ReP.Data;
using Altairis.ReP.Data.Commands.ReservationCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.ReservationCommandHandlers;
public class UpdateReservationCommandHandler : RepDbCommandHandler<Reservation, UpdateReservationCommand, SaveReservationCommandResult>
{
    public UpdateReservationCommandHandler(IConfigure<Reservation> configurator, IMapper mapper, IDbContextProxy proxy) : base(configurator, mapper, proxy)
    {
    }

    protected override async Task<SaveReservationCommandResult> GetResultToHandleAsync(UpdateReservationCommand command, CancellationToken token)
    {
        var result = new SaveReservationCommandResult
        {
            Conflicts = await GetEnumerableAsync<ReservationConflictDto>(
        r => r.DateBegin < command.DateEnd && r.DateEnd > command.DateBegin && r.Id != command.Id && r.ResourceId == command.ResourceId , token)
        };

        command.StatusChanged += (o, e) => result.Status = e.NewStatus;

        if (result.Conflicts.Any())
        {
            command.Status = CommandStatus.Conflict;
            return result;
        }

        var reservation = await GetOneOrNullAsync(r => r.Id == command.Id, token);

        if (reservation.DateBegin == command.DateBegin
            && reservation.DateEnd == command.DateEnd
            && reservation.System == command.System
            && reservation.Comment == command.Comment)
        {
            command.Status = CommandStatus.Unchanged;
            return result;
        }
       
        command.Status = await UpdateAndSaveAsync(MapCommandToExistingEntity(command, reservation), token);

        return result;
    }
}
