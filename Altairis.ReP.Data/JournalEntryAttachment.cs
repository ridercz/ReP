using System.ComponentModel.DataAnnotations.Schema;

namespace Altairis.ReP.Data;

public class JournalEntryAttachment : IAttachment {

    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime DateCreated { get; set; }

    [Required, MaxLength(100)]
    public string StoragePath { get; set; } = string.Empty;

    [ForeignKey(nameof(JournalEntry))]
    public int JournalEntryId { get; set; }

    [ForeignKey(nameof(JournalEntryId))]
    public virtual JournalEntry? JournalEntry { get; set; }

}
