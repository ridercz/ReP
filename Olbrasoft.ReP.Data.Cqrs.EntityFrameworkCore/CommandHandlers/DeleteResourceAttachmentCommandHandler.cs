using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public class DeleteResourceAttachmentCommandHandler : CommandHandler<ResourceAttachment, DeleteResourceAttachmentCommand, CommandStatus>
{
    public DeleteResourceAttachmentCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteResourceAttachmentCommand command, CancellationToken token)
    {
        var resourceAttachment = await SingleOrDefaultAsync(ra => ra.Id == command.ResourceAttachmentId, token);

        return resourceAttachment is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(resourceAttachment, token);
    }
}
