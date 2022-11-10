using Altairis.ReP.Data.Commands.OpenningHoursChangeCommands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.OpenningHoursChangeCommandHandlers;
public class SaveOpeningHoursChangeCommandHandler : RepDbCommandHandler<OpeningHoursChange, SaveOpeningHoursChangeCommand, CommandStatus>
{
    public SaveOpeningHoursChangeCommandHandler(IMapper mapper, RepDbContext context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveOpeningHoursChangeCommand command, CancellationToken token)
    {
        var openingHoursChange = await GetOneOrNullAsync(x => x.Date == command.Date, token);

        if (openingHoursChange is null) return await AddAndSaveAsync(MapCommandToNewEntity(command), token);

        return await SaveAsync(MapCommandToExistingEntity(command, openingHoursChange), token);
    }
}
