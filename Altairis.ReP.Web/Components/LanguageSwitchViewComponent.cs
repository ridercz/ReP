using System.Globalization;
using System.Resources;
using Microsoft.AspNetCore.Localization;

namespace Altairis.ReP.Web.Components;

public record struct SupportedLanguageInfo(string RequestCultureName, string LanguageName, string FlagCode, bool IsCurrent);

public class LanguageSwitchViewComponent : ViewComponent {
    public const string SetCultureCookieHandlerRouteName = "SetCookie";

    public IViewComponentResult Invoke() {
        var model = new Lazy<IEnumerable<SupportedLanguageInfo>>(() => GetAvailableCultures()
            .Select(c => new SupportedLanguageInfo(
                RequestCultureName: c.Name,
                LanguageName: c.NativeName,
                FlagCode: c.Name[^2..].ToLowerInvariant(),
                IsCurrent: CultureInfo.CurrentUICulture.Name.Equals(c.Name)))
        );
        return this.View(model);
    }

    public static IEnumerable<CultureInfo> GetAvailableCultures() {
        var result = new List<CultureInfo>();
        var rm = new ResourceManager(typeof(UI));
        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (var culture in cultures) {
            try {
                if (culture.Equals(CultureInfo.InvariantCulture)) continue;
                var rs = rm.GetResourceSet(culture, createIfNotExists: true, tryParents: false);
                if (rs != null) result.Add(culture);
            } catch (CultureNotFoundException) {
                //NOP
            }
        }
        return result;
    }

    public static IResult SetCultureCookieHandler(string culture, string returnUrl, HttpResponse rp) {
        var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
        var cookieName = CookieRequestCultureProvider.DefaultCookieName;
        rp.Cookies.Append(cookieName, cookieValue, new CookieOptions { MaxAge = TimeSpan.FromDays(365), IsEssential = true });
        return Results.LocalRedirect(returnUrl);
    }
}