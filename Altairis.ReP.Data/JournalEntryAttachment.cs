using System.ComponentModel.DataAnnotations.Schema;

namespace Altairis.ReP.Data;

public class JournalEntryAttachment {

    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(JournalEntry))]
    public int JournalEntryId { get; set; }

    [ForeignKey(nameof(JournalEntryId))]
    public virtual JournalEntry? JournalEntry { get; set; }

    [Required, MaxLength(100)]
    public required string FileName { get; set; }

    public long FileSize { get; set; }

    public DateTime DateCreated { get; set; }

    [Required, MaxLength(100)]
    public required string StoragePath { get; set; }


}
