using Altairis.ReP.Data.Commands.ResourceAttachmentCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.ResourceAttachmentCommandHandlers;
public class SaveResourceAttachmentCommandHandler : RepDbCommandHandler<ResourceAttachment, SaveResourceAttachmentCommand, ResourceAttachment>
{
    public SaveResourceAttachmentCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<ResourceAttachment> GetResultToHandleAsync(SaveResourceAttachmentCommand command, CancellationToken token)
    {
        var resourceAttachment = MapCommandToNewEntity(command);

        await AddAndSaveAsync(resourceAttachment, token);

        return resourceAttachment;
    }
}