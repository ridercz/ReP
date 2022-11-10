using Altairis.ReP.Data.Commands.ResourceAttachmentCommands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.ResourceAttachmentCommandHandlers;
public class DeleteResourceAttachmentCommandHandler : RepDbCommandHandler<ResourceAttachment, DeleteResourceAttachmentCommand, CommandStatus>
{
    public DeleteResourceAttachmentCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteResourceAttachmentCommand command, CancellationToken token) 
        => await RemoveAndSaveAsync(ra => ra.Id == command.ResourceAttachmentId, token);
}
