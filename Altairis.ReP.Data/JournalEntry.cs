using System.ComponentModel.DataAnnotations.Schema;

namespace Altairis.ReP.Data;

public class JournalEntry {

    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime DateCreated { get; set; }

    [ForeignKey(nameof(Resource))]
    public int? ResourceId { get; set; }

    [ForeignKey(nameof(ResourceId))]
    public virtual Resource? Resource { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Text { get; set; } = string.Empty;

    // Navigation properties

    public virtual ICollection<JournalEntryAttachment> Attachments { get; set; } = [];

}
