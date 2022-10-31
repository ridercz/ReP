using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class SaveOpeningHoursChangeCommandHandler : CommandHandlerWithMapper<OpeningHoursChange, SaveOpeningHoursChangeCommand, CommandStatus>
{
    public SaveOpeningHoursChangeCommandHandler(IMapper mapper, RepDbContext context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveOpeningHoursChangeCommand command, CancellationToken token)
    {
        var openingHoursChange = await SingleOrDefaultAsync(x => x.Date == command.Date, token);

        if (openingHoursChange is null) return await AddAndSaveAsync(MapCommandToNewEntity(command), token);

        return await SaveAsync(MapCommandToExistingEntity(command, openingHoursChange), token);
    }
}
