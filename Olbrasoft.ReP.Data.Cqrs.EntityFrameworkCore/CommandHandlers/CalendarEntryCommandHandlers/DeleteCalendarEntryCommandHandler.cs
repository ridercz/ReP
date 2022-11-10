namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.CalendarEntryCommandHandlers;
public class DeleteCalendarEntryCommandHandler : RepDbCommandHandler<CalendarEntry, DeleteCalendarEntryCommand, CommandStatus>
{
    public DeleteCalendarEntryCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteCalendarEntryCommand command, CancellationToken token) 
        => await RemoveAndSaveAsync(ce => ce.Id == command.Id, token);
}
