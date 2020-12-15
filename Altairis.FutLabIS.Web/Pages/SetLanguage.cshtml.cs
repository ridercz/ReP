using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.FutLabIS.Web.Pages {
    public class SetLanguageModel : PageModel {
        public IActionResult OnGet(string culture, string returnUrl = "/") {
            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture.Equals("cs") ? "cs-CZ" : "en-US"));
            var cookieName = CookieRequestCultureProvider.DefaultCookieName;
            this.Response.Cookies.Append(cookieName, cookieValue, new CookieOptions { MaxAge = TimeSpan.FromDays(365), IsEssential = true });
            return this.LocalRedirect(returnUrl);
        }
    }
}
