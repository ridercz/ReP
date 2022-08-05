namespace Altairis.ReP.Data;

public class CalendarEntry {

    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required, MaxLength(50)]
    public string Title { get; set; }

    public string Comment { get; set; }

}
