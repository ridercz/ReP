namespace Altairis.ReP.Data.Dtos;
public class UserInfoDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Language { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool Enabled { get; set; }
    public bool EmailConfirmed { get; set; }
}
