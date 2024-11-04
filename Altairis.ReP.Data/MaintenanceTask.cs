using System.ComponentModel.DataAnnotations.Schema;

namespace Altairis.ReP.Data;

public class MaintenanceTask {

    [Key]
    public int Id { get; set; }

    [Required, ForeignKey("Resource")]
    public int ResourceId { get; set; }

    [ForeignKey("ResourceId")]
    public Resource Resource { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Interval { get; set; } = string.Empty;

    public string? Description { get; set; }

}
