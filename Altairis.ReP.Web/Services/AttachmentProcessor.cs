using Altairis.ReP.Data.Entities;
using Altairis.Services.DateProvider;
using Storage.Net.Blobs;

namespace Altairis.ReP.Web.Services;
public class AttachmentProcessor
{
    private const string AttachmentPath = "attachments/{0:0000}/{1:yyyyMMddHHmmss}-{2:n}{3}";

    private readonly IBlobStorage blobStorage;
    private readonly IDateProvider dateProvider;
    private readonly IResourceAttachmentService _service;

    public AttachmentProcessor(IBlobStorage blobStorage, IDateProvider dateProvider, IResourceAttachmentService service)
    {
        this.blobStorage = blobStorage ?? throw new ArgumentNullException(nameof(blobStorage));
        this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<ResourceAttachment> CreateAttachment(IFormFile formFile, int resourceId)
    {

        var created = this.dateProvider.Now;
        var storagePath = string.Format(AttachmentPath,
             resourceId,                            // 0
             created,                               // 1
             Guid.NewGuid(),                        // 2
             Path.GetExtension(formFile.FileName)); //3

        // Upload file to storage
        using var stream = formFile.OpenReadStream();
        await this.blobStorage.WriteAsync(storagePath, stream);

        // Create attachment
        return await _service.SaveResourceAttachmentAsync(created,
                                                   resourceId,
                                                   Path.GetFileName(formFile.FileName),
                                                   formFile.Length,
                                                   storagePath
                                                   );
    }

    public async Task DeleteAttachment(int resourceAttachmentId)
    {
        // Get attachment info
        var a = await _service.GetAttachmentOrNullByAsync(resourceAttachmentId);
        if (a == null) return; // Already deleted

        // Delete attachment from storage
        await this.blobStorage.DeleteAsync(a.StoragePath);

        // Delete attachment from database
        await _service.DeleteResourceAttachmentAsync(resourceAttachmentId);
    }

    public async Task<(byte[], string)> GetAttachment(int resourceAttachmentId)
    {
        // Get attachment info
        var a = await _service.GetAttachmentOrNullByAsync(resourceAttachmentId);
        if (a == null) throw new FileNotFoundException();

        // Get data from storage
        var data = await this.blobStorage.ReadBytesAsync(a.StoragePath);

        // Send data
        return new(data, a.FileName);
    }
}
