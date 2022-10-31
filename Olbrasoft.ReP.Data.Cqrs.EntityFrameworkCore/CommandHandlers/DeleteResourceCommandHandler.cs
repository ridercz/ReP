using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class DeleteResourceCommandHandler : CommandHandler<Resource, DeleteResourceCommand, CommandStatus>
{
    public DeleteResourceCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteResourceCommand command, CancellationToken token)
        => await AnyAsync(r => r.Id == command.ResourceId, token)
           ? await RemoveAndSaveAsync(new Resource { Id = command.ResourceId }, token)
           : CommandStatus.NotFound;
}
