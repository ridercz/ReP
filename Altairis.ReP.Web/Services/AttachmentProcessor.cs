﻿using Altairis.Services.DateProvider;
using Storage.Net.Blobs;

namespace Altairis.ReP.Web.Services;
public class AttachmentProcessor {
    private const string AttachmentPath = "attachments/{0:0000}/{1:yyyyMMddHHmmss}-{2:n}{3}";

    private readonly IBlobStorage blobStorage;
    private readonly IDateProvider dateProvider;
    private readonly RepDbContext dc;

    public AttachmentProcessor(IBlobStorage blobStorage, IDateProvider dateProvider, RepDbContext dc) {
        this.blobStorage = blobStorage ?? throw new ArgumentNullException(nameof(blobStorage));
        this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        this.dc = dc;
    }

    public async Task<ResourceAttachment> CreateAttachment(IFormFile formFile, int resourceId) {
        // Create attachment
        var newAttachment = new ResourceAttachment {
            DateCreated = this.dateProvider.Now,
            FileName = Path.GetFileName(formFile.FileName),
            FileSize = formFile.Length,
            ResourceId = resourceId
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
        // Get attachment
        var a = await this.dc.ResourceAttachments.FindAsync(resourceAttachmentId);
        if (a == null) return; // Already deleted

        // Delete attachment from storage
        await this.blobStorage.DeleteAsync(a.StoragePath);

        // Delete attachment from database
        this.dc.ResourceAttachments.Remove(a);
        await this.dc.SaveChangesAsync();
    }

}