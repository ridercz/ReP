using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface IResourceAttachmentService
{
    Task<IEnumerable<ResourceAttachment>> GetResourceAttachmentsByAsync(int resourceId, CancellationToken token = default);
    Task<ResourceAttachment> SaveResourceAttachmentAsync(DateTime dateCreated,
                                                    int resourceId,
                                                    string fileName,
                                                    long fileSize,
                                                    string storagePath,
                                                    CancellationToken token = default);
    Task<AttachmentDto?> GetAttachmentOrNullByAsync(int resourceAttachmentId, CancellationToken token = default);

    Task<CommandStatus> DeleteResourceAttachmentAsync(int resourceAttachmentId, CancellationToken token = default);

}
