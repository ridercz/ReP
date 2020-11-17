using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Altairis.FutLabIS.Web.Services {
    public class ApplicationSignInManager : SignInManager<ApplicationUser> {
        public ApplicationSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) { }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure) {
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

            // Allow sign-in using e-mail address, not only user name
            if (result == SignInResult.Failed && userName.Contains('@')) {
                var userFoundByEmail = await this.UserManager.FindByEmailAsync(userName);
                if (userFoundByEmail != null) result = await base.PasswordSignInAsync(userFoundByEmail, password, isPersistent, lockoutOnFailure);
            }
            return result;
        }

        public override async Task<bool> CanSignInAsync(ApplicationUser user) {
            if (user is null) throw new System.ArgumentNullException(nameof(user));

            return await base.CanSignInAsync(user).ConfigureAwait(false)
                ? user.Enabled
                : false;
        }

    }
}
