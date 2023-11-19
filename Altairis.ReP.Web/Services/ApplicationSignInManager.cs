using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Web.Services;

#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
public class ApplicationSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation) : SignInManager<ApplicationUser>(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) {
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.

    public override async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure) {
        var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

        // Allow sign-in using e-mail address, not only user name
        if (result == Microsoft.AspNetCore.Identity.SignInResult.Failed && userName.Contains('@')) {
            var userFoundByEmail = await userManager.FindByEmailAsync(userName);
            if (userFoundByEmail != null) result = await base.PasswordSignInAsync(userFoundByEmail, password, isPersistent, lockoutOnFailure);
        }
        return result;
    }

    public override async Task<bool> CanSignInAsync(ApplicationUser user) {
        ArgumentNullException.ThrowIfNull(user);

        return await base.CanSignInAsync(user) && user.Enabled;
    }

}
