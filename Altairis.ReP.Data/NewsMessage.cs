namespace Altairis.ReP.Data;

public class NewsMessage {
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [Required, MaxLength(100)]
    public required string Title { get; set; }

    [Required]
    public required string Text { get; set; }

}
