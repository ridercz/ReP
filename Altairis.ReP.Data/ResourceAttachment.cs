using System.ComponentModel.DataAnnotations.Schema;

namespace Altairis.ReP.Data;

public class ResourceAttachment : IAttachment {

    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime DateCreated { get; set; }

    [Required, MaxLength(100)]
    public string StoragePath { get; set; } = string.Empty;

    [ForeignKey(nameof(this.Resource))]
    public int ResourceId { get; set; }

    [ForeignKey(nameof(this.ResourceId))]
    public virtual Resource? Resource { get; set; }

}
