namespace Altairis.ReP.Web.Pages.My;
public class OpeningHoursModel(OpeningHoursProvider hoursProvider) : PageModel {
    private readonly OpeningHoursProvider hoursProvider = hoursProvider;

    // Output model

    public IEnumerable<OpeningHoursInfo> OpeningHours => this.hoursProvider.GetOpeningHours(0, 14);

}
