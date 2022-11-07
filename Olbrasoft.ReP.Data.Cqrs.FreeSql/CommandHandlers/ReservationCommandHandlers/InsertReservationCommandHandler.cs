using Altairis.ReP.Data;
using Altairis.ReP.Data.Commands.ReservationCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.ReservationCommandHandlers;
public class InsertReservationCommandHandler : RepDbCommandHandler<Reservation, InsertReservationCommand, SaveReservationCommandResult>
{
    public InsertReservationCommandHandler(IMapper mapper, IDbContextProxy proxy) : base(mapper, proxy)
    {
    }

    protected override async Task<SaveReservationCommandResult> GetResultToHandleAsync(InsertReservationCommand command, CancellationToken token)
    {
        var result = new SaveReservationCommandResult
        {
            Conflicts = await
                GetWhere(r => r.ResourceId == command.ResourceId && r.DateBegin < command.DateEnd && r.DateEnd > command.DateBegin)
            .ToListAsync(r => new ReservationConflictDto { UserName = r.User!.UserName }, token)
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
