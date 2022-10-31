using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class DeleteOpeningHoursChangeCommandHandler : CommandHandler<OpeningHoursChange, DeleteOpeningHoursChangeCommand, CommandStatus>
{
    public DeleteOpeningHoursChangeCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteOpeningHoursChangeCommand command, CancellationToken token)
    {
        var openingHoursChange = await SingleOrDefaultAsync(ohch => ohch.Id == command.OpeningHoursChangeId, token);

        return openingHoursChange is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(openingHoursChange, token);
    }
}
