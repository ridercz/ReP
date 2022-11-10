namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.OpenningHoursChangeCommandHandlers;
public class DeleteOpeningHoursChangeCommandHandler : RepDbCommandHandler<OpeningHoursChange, DeleteOpeningHoursChangeCommand, CommandStatus>
{
    public DeleteOpeningHoursChangeCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteOpeningHoursChangeCommand command, CancellationToken token) 
        => await RemoveAndSaveAsync(o => o.Id == command.OpeningHoursChangeId, token);
}
