using System.Security.Principal;

namespace Altairis.ReP.Data.Entities;

[Table("CalendarEntries")]

public class CalendarEntry
{

    [Key]
  
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required, MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    public string? Comment { get; set; }

}
