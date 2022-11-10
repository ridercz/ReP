using Altairis.ReP.Data.Commands.CalendarEntryCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.CalendarEntryCommandHandlers;
public class SaveCalendarEntryCommandHandler : RepDbCommandHandler<CalendarEntry, SaveCalendarEntryCommand, CommandStatus>
{
    public SaveCalendarEntryCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveCalendarEntryCommand command, CancellationToken token)
        => await AddAndSaveAsync(MapCommandToNewEntity(command), token);
}