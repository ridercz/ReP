using Altairis.ReP.Data.Commands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.CalendarEntryCommandHandlers;
public class SaveCalendarEntryCommandHandler : CommandHandlerWithMapper<CalendarEntry, SaveCalendarEntryCommand, CommandStatus>
{
    public SaveCalendarEntryCommandHandler(IMapper mapper, RepDbContext context) : base(mapper, context)
    {}

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveCalendarEntryCommand command, CancellationToken token) 
        => await AddAndSaveAsync(MapCommandToNewEntity(command), token);
}