namespace Altairis.ReP.Data.Dtos.ReservationDtos;
public class ReservationFullInfoDto
{
    public int Id { get; set; }

    public string UserDisplayName { get; set; } = string.Empty;

    public string ResourceName { get; set; } = string.Empty;

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public bool System { get; set; }

    public string? Comment { get; set; }
}
