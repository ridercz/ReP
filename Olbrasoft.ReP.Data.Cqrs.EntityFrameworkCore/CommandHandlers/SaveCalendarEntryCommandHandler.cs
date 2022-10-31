using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class SaveCalendarEntryCommandHandler : CommandHandlerWithMapper<CalendarEntry, SaveCalendarEntryCommand, CommandStatus>
{
    public SaveCalendarEntryCommandHandler(IMapper mapper, RepDbContext context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveCalendarEntryCommand command, CancellationToken token)
    {
        return await AddAndSaveAsync(MapCommandToNewEntity(command), token);
    }
}