namespace Altairis.ReP.Data;

public class NewsMessage {
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public string Text { get; set; }

}
