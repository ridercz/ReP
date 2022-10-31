using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class DeleteCalendarEntryCommandHandler : CommandHandler<CalendarEntry, DeleteCalendarEntryCommand,CommandStatus>
{
    public DeleteCalendarEntryCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteCalendarEntryCommand command, CancellationToken token)
    {
        return await RemoveAndSaveAsync(await SingleAsync(ce => ce.Id == command.Id, token), token);
    }
}
