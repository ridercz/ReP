using System;
using System.Collections.Generic;
using Altairis.FutLabIS.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages.My {
    public class OpeningHoursModel : PageModel {
        private readonly OpeningHoursProvider hoursProvider;

        public OpeningHoursModel(OpeningHoursProvider hoursProvider) {
            this.hoursProvider = hoursProvider ?? throw new ArgumentNullException(nameof(hoursProvider));
        }

        public IEnumerable<OpeningHoursInfo> OpeningHours => this.hoursProvider.GetOpeningHours(0, 14);

    }
}
