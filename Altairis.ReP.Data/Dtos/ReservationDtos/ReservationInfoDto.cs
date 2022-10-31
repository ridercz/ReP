namespace Altairis.ReP.Data.Dtos.ReservationDtos;

public class ReservationInfoDto
{
    public int Id { get; set; }

    public int ResourceId { get; set; }

    public string ResourceName { get; set; } = string.Empty;

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public bool CanBeDeleted { get; set; }
}
