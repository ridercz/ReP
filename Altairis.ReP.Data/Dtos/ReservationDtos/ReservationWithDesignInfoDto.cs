namespace Altairis.ReP.Data.Dtos.ReservationDtos;

public class ReservationWithDesignInfoDto
{
    public int Id { get; set; }

    public bool System { get; set; }

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public string? Comment { get; set; } = string.Empty;

    public string ResourceForegroundColor { get; set; } = string.Empty;

    public string ResourceBackgroundColor { get; set; } = string.Empty;

    public string UserDisplayName { get; set; } = string.Empty;

}
