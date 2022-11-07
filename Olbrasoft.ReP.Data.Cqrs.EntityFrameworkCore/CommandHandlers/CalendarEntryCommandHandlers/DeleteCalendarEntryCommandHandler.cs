using Altairis.ReP.Data.Commands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.CalendarEntryCommandHandlers;
public class DeleteCalendarEntryCommandHandler : DbCommandHandler<CalendarEntry, DeleteCalendarEntryCommand, CommandStatus>
{
    public DeleteCalendarEntryCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteCalendarEntryCommand command, CancellationToken token)
    {
        return await RemoveAndSaveAsync(await SingleAsync(ce => ce.Id == command.Id, token), token);
    }
}
