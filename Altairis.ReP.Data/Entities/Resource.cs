namespace Altairis.ReP.Data.Entities;

[Table("Resources")]
public class Resource
{

    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [DataType("Markdown")]
    public string? Instructions { get; set; }

    [Required, Range(0, 1440)]
    public int MaximumReservationTime { get; set; }

    public bool Enabled { get; set; } = true;

    [Required, MinLength(7), MaxLength(7), RegularExpression(@"^\#[0-9A-Fa-f]{6}$")]
    public string ForegroundColor { get; set; } = "#000000";

    [Required, MinLength(7), MaxLength(7), RegularExpression(@"^\#[0-9A-Fa-f]{6}$")]
    public string BackgroundColor { get; set; } = "#ffffff";

    public IEnumerable<ResourceAttachment> Attachments { get; set; } = new HashSet<ResourceAttachment>();

}
