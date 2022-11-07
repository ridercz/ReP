using Altairis.ReP.Data.Commands.ReservationCommands;
using Altairis.ReP.Data.Dtos.ReservationDtos;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.ReservationCommandHandlers;
public class UpdateReservationCommandHandler : CommandHandlerWithMapperAndProjector<Reservation, UpdateReservationCommand, SaveReservationCommandResult>
{
    public UpdateReservationCommandHandler(IProjector projector, IMapper mapper, RepDbContext context) : base(projector, mapper, context)
    {
    }

    protected override async Task<SaveReservationCommandResult> GetResultToHandleAsync(UpdateReservationCommand command, CancellationToken token)
    {
        var result = new SaveReservationCommandResult
        {
            Conflicts = await ProjectTo<ReservationConflictDto>(
                Where(r => r.DateBegin < command.DateEnd && r.DateEnd > command.DateBegin && r.Id != command.Id))
          .ToArrayAsync(token)
        };

        command.StatusChanged += (o, e) => result.Status = e.NewStatus;

        if (result.Conflicts.Any())
        {
            command.Status = CommandStatus.Conflict;
            return result;
        }

        var reservation = await SingleAsync(r => r.Id == command.Id, token);

        command.Status = await SaveAsync(MapCommandToExistingEntity(command, reservation), token);

        return result;
    }
}
