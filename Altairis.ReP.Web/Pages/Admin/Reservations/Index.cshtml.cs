using Altairis.ReP.Data.Dtos.ReservationDtos;

namespace Altairis.ReP.Web.Pages.Admin.Reservations;
public class IndexModel : PageModel {
    private readonly IReservationService _service;

    public IndexModel(IReservationService service) 
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    public IEnumerable<ReservationFullInfoDto> Reservations { get; set; }

    public async Task OnGetAsync(CancellationToken token) 
        => this.Reservations = await _service.GetReservationFullInfosAsync(token);
}