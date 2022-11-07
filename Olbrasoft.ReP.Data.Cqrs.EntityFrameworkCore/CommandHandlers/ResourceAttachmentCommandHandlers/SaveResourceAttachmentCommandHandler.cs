using Altairis.ReP.Data.Commands;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers.ResourceAttachmentCommandHandlers;
public class SaveResourceAttachmentCommandHandler : CommandHandlerWithMapper<ResourceAttachment, SaveResourceAttachmentCommand, ResourceAttachment>
{
    public SaveResourceAttachmentCommandHandler(IMapper mapper, RepDbContext context) : base(mapper, context)
    {
    }

    protected override async Task<ResourceAttachment> GetResultToHandleAsync(SaveResourceAttachmentCommand command, CancellationToken token)
    {
        var resourceAttachment = MapCommandToNewEntity(command);

        await AddAndSaveAsync(resourceAttachment, token);

        return resourceAttachment;
    }
}