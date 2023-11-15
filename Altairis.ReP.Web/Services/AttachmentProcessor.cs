using Altairis.Services.DateProvider;
using FluentStorage.Blobs;

namespace Altairis.ReP.Web.Services;
public class AttachmentProcessor(IBlobStorage blobStorage, IDateProvider dateProvider, RepDbContext dc) {
    private const string AttachmentPath = "attachments/{0:0000}/{1:yyyyMMddHHmmss}-{2:n}{3}";

    private readonly IBlobStorage blobStorage = blobStorage ?? throw new ArgumentNullException(nameof(blobStorage));
    private readonly IDateProvider dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
    private readonly RepDbContext dc = dc;

    public async Task<ResourceAttachment> CreateAttachment(IFormFile formFile, int resourceId) {
        // Create attachment
        var newAttachment = new ResourceAttachment {
            DateCreated = this.dateProvider.Now,
            FileName = Path.GetFileName(formFile.FileName),
            FileSize = formFile.Length,
            ResourceId = resourceId,
            StoragePath = string.Empty
        };

        // Create attachment storage path
        newAttachment.StoragePath = string.Format(AttachmentPath,
            newAttachment.ResourceId,               // 0
            newAttachment.DateCreated,              // 1
            Guid.NewGuid(),                         // 2
            Path.GetExtension(formFile.FileName));  // 3

        // Upload file to storage
        using var stream = formFile.OpenReadStream();
        await this.blobStorage.WriteAsync(newAttachment.StoragePath, stream);

        // Save to database
        await this.dc.ResourceAttachments.AddAsync(newAttachment);
        await this.dc.SaveChangesAsync();

        // Return data entity
        return newAttachment;
    }

    public async Task DeleteAttachment(int resourceAttachmentId) {
        // Get attachment info
        var a = await this.dc.ResourceAttachments.FindAsync(resourceAttachmentId);
        if (a == null) return; // Already deleted

        // Delete attachment from storage
        await this.blobStorage.DeleteAsync(a.StoragePath);

        // Delete attachment from database
        this.dc.ResourceAttachments.Remove(a);
        await this.dc.SaveChangesAsync();
    }

    public async Task<(byte[], string)> GetAttachment(int resourceAttachmentId) {
        // Get attachment info
        var a = await this.dc.ResourceAttachments.FindAsync(resourceAttachmentId) ?? throw new FileNotFoundException();

        // Get data from storage
        var data = await this.blobStorage.ReadBytesAsync(a.StoragePath);

        // Send data
        return new(data, a.FileName);
    }

}
