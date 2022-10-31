namespace Altairis.ReP.Data.Entities;

[Table("ResourceAttachments")]
public class ResourceAttachment
{

    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Resource))]
    public int ResourceId { get; set; }

    [ForeignKey(nameof(ResourceId))]
    public Resource Resource { get; set; }

    [Required, MaxLength(100)]
    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime DateCreated { get; set; }

    [Required, MaxLength(100)]
    public string StoragePath { get; set; } = string.Empty;

}
