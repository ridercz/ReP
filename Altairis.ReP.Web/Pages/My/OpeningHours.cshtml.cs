namespace Altairis.ReP.Web.Pages.My {
    public class OpeningHoursModel : PageModel {
        private readonly OpeningHoursProvider hoursProvider;

        public OpeningHoursModel(OpeningHoursProvider hoursProvider) {
            this.hoursProvider = hoursProvider ?? throw new ArgumentNullException(nameof(hoursProvider));
        }

        public IEnumerable<OpeningHoursInfo> OpeningHours => this.hoursProvider.GetOpeningHours(0, 14);

    }
}
