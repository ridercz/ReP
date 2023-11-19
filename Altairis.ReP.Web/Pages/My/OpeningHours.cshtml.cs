namespace Altairis.ReP.Web.Pages.My;

public class OpeningHoursModel(OpeningHoursProvider hoursProvider) : PageModel {

    // Output model

    public IEnumerable<OpeningHoursInfo> OpeningHours => hoursProvider.GetOpeningHours(0, 14);

}
