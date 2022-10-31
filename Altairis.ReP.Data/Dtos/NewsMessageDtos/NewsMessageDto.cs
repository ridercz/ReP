namespace Altairis.ReP.Data.Dtos.NewsMessageDtos;
public class NewsMessageDto
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, DataType("Markdown")]
    public string Text { get; set; } = string.Empty;

}
