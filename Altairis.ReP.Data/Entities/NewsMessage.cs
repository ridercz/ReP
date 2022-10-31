namespace Altairis.ReP.Data.Entities;

[Table("NewsMessages")]
public class NewsMessage
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Text { get; set; } = string.Empty;

}
