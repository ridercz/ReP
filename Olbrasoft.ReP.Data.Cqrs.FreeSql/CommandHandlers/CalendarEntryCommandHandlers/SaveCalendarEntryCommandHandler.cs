namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.CalendarEntryCommandHandlers;
public class SaveCalendarEntryCommandHandler : RepDbCommandHandler<CalendarEntry, SaveCalendarEntryCommand, CommandStatus>
{
    public SaveCalendarEntryCommandHandler(IMapper mapper, IDbContextProxy proxy) : base(mapper, proxy)
    {}

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveCalendarEntryCommand command, CancellationToken token)
        => await AddAndSaveAsync(MapCommandToNewEntity(command), token);

}