using Altairis.ReP.Data.Commands.ReservationCommands;
using Altairis.ReP.Data.Dtos.ReservationDtos;
using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.ReservationCommandHandlers;
public class SaveReservationCommandHandler : RepDbCommandHandler<Reservation, SaveReservationCommand, SaveReservationCommandResult>
{
    public SaveReservationCommandHandler(IProjector projector, IMapper mapper, RepDbContext context) : base(projector, mapper, context)
    {
    }

    protected override async Task<SaveReservationCommandResult> GetResultToHandleAsync(SaveReservationCommand command, CancellationToken token)
    {
        var result = new SaveReservationCommandResult();

        command.StatusChanged += (o, e) => result.Status = e.NewStatus;

        Expression<Func<Reservation, bool>>
            predicate = 
            r => r.ResourceId == command.ResourceId && r.DateBegin < command.DateEnd && r.DateEnd > command.DateBegin;

        var reservation = MapCommandToNewEntity(command);

        if (command.Id > 0)
        {
            predicate =
            r => r.DateBegin < command.DateEnd && r.DateEnd > command.DateBegin && r.Id != command.Id && r.ResourceId == command.ResourceId;
            
            reservation = await GetOneOrNullAsync(r => r.Id == command.Id, token);

            if (reservation is null) return result;
        }

        result.Conflicts = await GetEnumerableAsync<ReservationConflictDto>(predicate, token);

        if (result.Conflicts.Any())
        {
            command.Status = CommandStatus.Conflict;
            return result;
        }

        if (command.Id <= 0)
        {
            command.Status = await AddAndSaveAsync(reservation, token);
        }
        else
        {   
            reservation.DateBegin = command.DateBegin;
            reservation.DateEnd = command.DateEnd;
            reservation.Comment = command.Comment;
            reservation.System = command.System;

            command.Status = await SaveAsync(reservation , token);
        }

        return result;
    }
}
