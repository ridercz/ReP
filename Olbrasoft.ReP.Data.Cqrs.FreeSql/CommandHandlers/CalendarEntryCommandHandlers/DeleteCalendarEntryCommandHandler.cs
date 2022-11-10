using Altairis.ReP.Data.Commands.CalendarEntryCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.CalendarEntryCommandHandlers;
public class DeleteCalendarEntryCommandHandler : RepDbCommandHandler<CalendarEntry, DeleteCalendarEntryCommand, CommandStatus>
{
    public DeleteCalendarEntryCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteCalendarEntryCommand command, CancellationToken token) 
        => await RemoveAndSaveAsync(ce => ce.Id == command.Id, token);
}
