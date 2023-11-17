namespace Altairis.ReP.Data;

public class DirectoryEntry {

    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(100), EmailAddress]
    public string? Email { get; set; }

    [MaxLength(50), Phone]
    public string? PhoneNumber { get; set; }

}
