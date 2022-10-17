using System.ComponentModel.DataAnnotations.Schema;

namespace Altairis.ReP.Data;

public class ResourceAttachment {

    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(this.Resource))]
    public int  ResourceId { get; set; }

    [ForeignKey(nameof(this.ResourceId))]
    public Resource Resource { get; set; }

    [Required, MaxLength(100)]
    public string FileName { get; set; }

    public long FileSize { get; set; }

    public DateTime DateCreated { get; set; }

    [Required, MaxLength(100)]
    public string StoragePath { get; set; }

}
