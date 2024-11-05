using System.ComponentModel.DataAnnotations.Schema;

namespace Altairis.ReP.Data;

public class MaintenanceRecord {

    [Key]
    public int Id { get; set; }

    [Required, ForeignKey(nameof(Resource))]
    public int ResourceId { get; set; }

    [ForeignKey(nameof(ResourceId))]
    public Resource Resource { get; set; } = null!;

    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    [Required, ForeignKey(nameof(MaintenanceTask))]
    public int MaintenanceTaskId { get; set; }

    [ForeignKey(nameof(MaintenanceTaskId))]
    public MaintenanceTask MaintenanceTask { get; set; } = null!;

    [Required]
    public DateTime Date { get; set; }

}
