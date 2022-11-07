using Altairis.ReP.Data.Commands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.OpenningHoursChangeCommandHandlers;
public class DeleteOpeningHoursChangeCommandHandler : DbCommandHandler<OpeningHoursChange, DeleteOpeningHoursChangeCommand, CommandStatus>
{
    public DeleteOpeningHoursChangeCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteOpeningHoursChangeCommand command, CancellationToken token)
    {
        var openingHoursChange = await GetOneOrNullAsync(ohch => ohch.Id == command.OpeningHoursChangeId, token);

        return openingHoursChange is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(openingHoursChange, token);
    }
}
