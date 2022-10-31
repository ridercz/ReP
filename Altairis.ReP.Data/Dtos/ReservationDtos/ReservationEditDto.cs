namespace Altairis.ReP.Data.Dtos.ReservationDtos;
public class ReservationEditDto
{
    public int Id { get; set; }
    public int ResourceId { get; set; }
    public string ResourceName { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool UserSendNotifications { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string UserLanguage { get; set; }= string.Empty;
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }
    public string? Comment { get; set; }
    public bool System { get; set; }
}
