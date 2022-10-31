using Altairis.ReP.Data.Commands.ReservationCommands;
using Altairis.ReP.Data.Dtos.ReservationDtos;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.ReservationCommandHandlers;
public class InsertReservationCommandHandler : CommandHandlerWithMapperAndProjector<Reservation, InsertReservationCommand, SaveReservationCommandResult>
{
    public InsertReservationCommandHandler(IProjector projector, IMapper mapper, RepDbContext context) : base(projector, mapper, context)
    {
    }

    protected override async Task<SaveReservationCommandResult> GetResultToHandleAsync(InsertReservationCommand command, CancellationToken token)
    {
        var result = new SaveReservationCommandResult
        {
            Conflicts = await ProjectTo<ReservationConflictDto>(
                Where(r => r.ResourceId == command.ResourceId && r.DateBegin < command.DateEnd && r.DateEnd > command.DateBegin))
            .ToArrayAsync(token)
        };

        command.StatusChanged += (o, e) => result.Status = e.NewStatus;

        if (result.Conflicts.Any())
        {
            command.Status = CommandStatus.Conflict;
            return result;
        }


        var r = MapCommandToNewEntity(command);

        command.Status = await AddAndSaveAsync(r, token);

        return result;
    }
}
