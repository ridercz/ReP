using FluentStorage.Blobs;
namespace Altairis.ReP.Web.Services;

public abstract class AttachmentProcessor<T>(IBlobStorage blobStorage, TimeProvider timeProvider, RepDbContext dc) where T : class, IAttachment, new() {

    // Abstract methods

    protected abstract void SetRelatedEntryId(T attachment, int relatedEntryId);

    protected abstract string GetStoragePath(T attachment);

    // Public methods

    public async Task<T> CreateAttachment(IFormFile formFile, int relatedEntryId) {
        // Create attachment
        var newAttachment = new T {
            DateCreated = timeProvider.GetLocalNow().DateTime,
            FileName = Path.GetFileName(formFile.FileName),
            FileSize = formFile.Length,
            StoragePath = string.Empty,
        };

        // Set related entry ID
        this.SetRelatedEntryId(newAttachment, relatedEntryId);

        // Create attachment storage path
        newAttachment.StoragePath = this.GetStoragePath(newAttachment);

        // Upload file to storage
        using var stream = formFile.OpenReadStream();
        await blobStorage.WriteAsync(newAttachment.StoragePath, stream);

        // Save to database
        dc.Entry(newAttachment).State = EntityState.Added;
        await dc.SaveChangesAsync();

        // Return data entity
        return newAttachment;
    }

    public async Task DeleteAttachment(int id) {
        // Get attachment info
        var attachment = await dc.FindAsync<T>(id);
        if (attachment == null) return; // Already deleted

        // Delete attachment from storage
        await blobStorage.DeleteAsync(attachment.StoragePath);

        // Delete attachment from database
        dc.Entry(attachment).State = EntityState.Deleted;
        await dc.SaveChangesAsync();
    }

    public async Task<(byte[], string)> GetAttachment(int id) {
        // Get attachment info
        var a = await dc.FindAsync<T>(id) ?? throw new FileNotFoundException();

        // Get data from storage
        var data = await blobStorage.ReadBytesAsync(a.StoragePath);

        // Send data
        return new(data, a.FileName);
    }

}

public class ResourceAttachmentProcessor(IBlobStorage blobStorage, TimeProvider timeProvider, RepDbContext dc) : AttachmentProcessor<ResourceAttachment>(blobStorage, timeProvider, dc) {
    private const string AttachmentPath = "attachments/{0:0000}/{1:yyyyMMddHHmmss}-{2:n}{3}";

    protected override void SetRelatedEntryId(ResourceAttachment attachment, int relatedEntryId) => attachment.ResourceId = relatedEntryId;

    protected override string GetStoragePath(ResourceAttachment attachment) => string.Format(AttachmentPath,
            attachment.ResourceId,                                      // 0
            attachment.DateCreated,                                     // 1
            Guid.NewGuid(),                                             // 2
            Path.GetExtension(attachment.FileName.ToLowerInvariant())); // 3
}

public class JournalAttachmentProcessor(IBlobStorage blobStorage, TimeProvider timeProvider, RepDbContext dc) : AttachmentProcessor<JournalEntryAttachment>(blobStorage, timeProvider, dc) {
    private const string AttachmentPath = "journals/{0:0000}/{1:yyyyMMddHHmmss}-{2:n}{3}";

    protected override void SetRelatedEntryId(JournalEntryAttachment attachment, int relatedEntryId) => attachment.JournalEntryId = relatedEntryId;

    protected override string GetStoragePath(JournalEntryAttachment attachment) => string.Format(AttachmentPath,
            attachment.JournalEntryId,                                  // 0
            attachment.DateCreated,                                     // 1
            Guid.NewGuid(),                                             // 2
            Path.GetExtension(attachment.FileName.ToLowerInvariant())); // 3
}
