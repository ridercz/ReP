using Altairis.ReP.Data.Commands.ResourceAttachmentCommands;

namespace Olbrasoft.ReP.Business;
public class ResourceAttachmentService : BaseService, IResourceAttachmentService
{
    public ResourceAttachmentService(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public Task<CommandStatus> DeleteResourceAttachmentAsync(int resourceAttachmentId, CancellationToken token = default) 
        => new DeleteResourceAttachmentCommand(Dispatcher) { ResourceAttachmentId = resourceAttachmentId }.ToResultAsync(token);

    public async Task<AttachmentDto?> GetAttachmentOrNullByAsync(int resourceAttachmentId, CancellationToken token = default)
        => await new AttachmentDtoByResourceAttachmentIdQuery(Dispatcher) { ResourceAttachmentId = resourceAttachmentId }.ToResultAsync(token);

    public async Task<IEnumerable<ResourceAttachment>> GetResourceAttachmentsByAsync(int resourceId, CancellationToken token)
        => await new ResourceAttachmentsByResourceIdQuery(Dispatcher) { ResourceId = resourceId }.ToResultAsync(token: token);

    public async Task<ResourceAttachment> SaveResourceAttachmentAsync(DateTime dateCreated,
                                                           int resourceId,
                                                           string fileName,
                                                           long fileSize,
                                                           string storagePath,
                                                           CancellationToken token = default)
        => await new SaveResourceAttachmentCommand(Dispatcher)
        {
            DateCreated = dateCreated,
            ResourceId = resourceId,
            FileName = fileName,
            FileSize = fileSize,
            StoragePath = storagePath
        
        }.ToResultAsync(token);
}
