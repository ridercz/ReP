namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.ResourceAttachmentCommandHandlers;
public class DeleteResourceAttachmentCommandHandler : RepDbCommandHandler<ResourceAttachment, DeleteResourceAttachmentCommand, CommandStatus>
{
    public DeleteResourceAttachmentCommandHandler(IMapper mapper, IDbContextProxy proxy) : base(mapper, proxy)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteResourceAttachmentCommand command, CancellationToken token)
    {
        var resourceAttachment = await GetOneOrNullAsync(ra => ra.Id == command.ResourceAttachmentId, token);

        return resourceAttachment is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(resourceAttachment, token);
    }
}
