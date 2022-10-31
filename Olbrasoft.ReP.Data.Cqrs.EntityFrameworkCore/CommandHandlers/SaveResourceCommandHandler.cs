using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class SaveResourceCommandHandler : CommandHandlerWithMapper<Resource, SaveResourceCommand, CommandStatus>
{
    public SaveResourceCommandHandler(IMapper mapper, RepDbContext context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(SaveResourceCommand command, CancellationToken token) 
        => command.Id <= 0
            ? await AddAndSaveAsync(MapCommandToNewEntity(command), token)
            : await AnyAsync(r => r.Id == command.Id, token)
            ? await UpdateAndSaveAsync(MapCommandToNewEntity(command), token)
            : CommandStatus.NotFound;
}
