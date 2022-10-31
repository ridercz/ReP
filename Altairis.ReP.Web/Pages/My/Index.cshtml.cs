using Altairis.ReP.Data.Dtos.ReservationDtos;
using Altairis.ReP.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Pages.My;
public partial class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly OpeningHoursProvider hoursProvider;
    private readonly IResourceService _resourceService;
    private readonly IReservationService _reservationService;
    private readonly INewsMessageService _newMesaageService;

    private int UserId => int.Parse(this.userManager.GetUserId(this.User));

    public IndexModel(UserManager<ApplicationUser> userManager,
                      OpeningHoursProvider hoursProvider,
                      IResourceService resourceService,
                      IReservationService reservationService,
                      INewsMessageService newMesaageService)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.hoursProvider = hoursProvider ?? throw new ArgumentNullException(nameof(hoursProvider));
        _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        _newMesaageService = newMesaageService ?? throw new ArgumentNullException(nameof(newMesaageService));
    }

    public IEnumerable<Resource> Resources { get; set; }

    public IEnumerable<ReservationInfoDto> Reservations { get; set; }

    public OpeningHoursInfo OpenToday { get; set; }

    public OpeningHoursInfo OpenTomorrow { get; set; }

    public DateTime LastNewsDate { get; set; }

    public string LastNewsTitle { get; set; }

    public string LastNewsText { get; set; }

    public async Task OnGetAsync(CancellationToken token)
    {
        // Get operning hours
        this.OpenToday = await this.hoursProvider.GetOpeningHours(0);
        this.OpenTomorrow = await this.hoursProvider.GetOpeningHours(1);

        // Get latest news message
        var latestNews = await _newMesaageService.GetFirstNewsMessageOrNullAsync(token);
        if (latestNews != null)
        {
            this.LastNewsDate = latestNews.Date;
            this.LastNewsTitle = latestNews.Title;
            this.LastNewsText = latestNews.Text;
        }

        // Get resources accessible to user
        this.Resources = await _resourceService.GetResourcesAsync(this.User.IsPrivilegedUser(), token);

        this.Reservations = await _reservationService.GetReservationInfosAsync(this.UserId, token);
    }

    public async Task<IActionResult> OnGetDeleteAsync(int reservationId, CancellationToken token)
    {
        return (await _reservationService.DeleteReservationAsync(reservationId, UserId, token)) == CommandStatus.NotFound
           ? this.NotFound()
           : this.RedirectToPage("Index", null, "reservationdeleted");
    }
}
