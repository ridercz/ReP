using Altairis.ReP.Data.Commands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.ResourceAttachmentCommandHandlers;
public class DeleteResourceAttachmentCommandHandler : DbCommandHandler<ResourceAttachment, DeleteResourceAttachmentCommand, CommandStatus>
{
    public DeleteResourceAttachmentCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteResourceAttachmentCommand command, CancellationToken token)
    {
        var resourceAttachment = await GetOneOrNullAsync(ra => ra.Id == command.ResourceAttachmentId, token);

        return resourceAttachment is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(resourceAttachment, token);
    }
}
