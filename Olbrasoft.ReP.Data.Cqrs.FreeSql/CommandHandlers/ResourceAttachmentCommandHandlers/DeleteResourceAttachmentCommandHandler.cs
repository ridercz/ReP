using Altairis.ReP.Data.Commands.ResourceAttachmentCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.ResourceAttachmentCommandHandlers;
public class DeleteResourceAttachmentCommandHandler : RepDbCommandHandler<ResourceAttachment, DeleteResourceAttachmentCommand, CommandStatus>
{
    public DeleteResourceAttachmentCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteResourceAttachmentCommand command, CancellationToken token)
    {
        var resourceAttachment = await GetOneOrNullAsync(ra => ra.Id == command.ResourceAttachmentId, token);

        return resourceAttachment is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(resourceAttachment, token);
    }
}
