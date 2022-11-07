namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.CalendarEntryCommandHandlers;
public class DeleteCalendarEntryCommandHandler : RepDbCommandHandler<CalendarEntry, DeleteCalendarEntryCommand, CommandStatus>
{
    public DeleteCalendarEntryCommandHandler(IMapper mapper, IDbContextProxy proxy) : base(mapper, proxy)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteCalendarEntryCommand command, CancellationToken token) 
        => await RemoveAndSaveAsync(ce => ce.Id == command.Id, token);
}
