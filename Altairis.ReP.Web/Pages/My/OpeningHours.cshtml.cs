namespace Altairis.ReP.Web.Pages.My;
public class OpeningHoursModel(OpeningHoursProvider hoursProvider) : PageModel {
    private readonly OpeningHoursProvider hoursProvider = hoursProvider ?? throw new ArgumentNullException(nameof(hoursProvider));

    public IEnumerable<OpeningHoursInfo> OpeningHours => this.hoursProvider.GetOpeningHours(0, 14);

}
