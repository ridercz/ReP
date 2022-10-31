namespace Altairis.ReP.Data.Dtos;
public class ResourceInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ForegroundColor { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;

    public string GetStyle() => $"color:{this.ForegroundColor};background-color:{this.BackgroundColor};";
}
