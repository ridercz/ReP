using Altairis.ReP.Data.Commands.ReservationCommands;
using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.ReservationCommandHandlers;
public class DeleteReservationCommandHandler : RepDbCommandHandler<Reservation, DeleteReservationCommand, CommandStatus>
{
    public DeleteReservationCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteReservationCommand command, CancellationToken token)
    {
        Expression<Func<Reservation, bool>> predicate = r => r.Id == command.ResevationId;

        if (command.UserId > 0)
        {
            predicate = r => r.Id == command.ResevationId && r.UserId == command.UserId && r.DateEnd > command.Now;
        }

        var reservation = await GetOneOrNullAsync(predicate, token);

        return reservation is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(reservation, token);
    }
}
