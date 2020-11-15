using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.FutLabIS.Data;
using Altairis.Services.PwnedPasswordsValidator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Altairis.FutLabIS.Web {
    public class Startup {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;
        private readonly AppSettings appSettings;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            this.configuration = configuration;
            this.environment = environment;
            this.appSettings = new AppSettings();
            this.configuration.Bind(this.appSettings);
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<FutLabDbContext>(options => {
                options.UseSqlServer(this.configuration.GetConnectionString("FutLabIS"));
            });
            services.AddRazorPages();

            // Configure identity
            services.AddAuthorization(options => {
                options.AddPolicy("IsLoggedIn", policy => policy.RequireAuthenticatedUser());
                options.AddPolicy("IsMaster", policy => policy.RequireRole(ApplicationRole.Master));
                options.AddPolicy("IsAdministrator", policy => policy.RequireRole(ApplicationRole.Administrator));
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Password.RequiredLength = this.appSettings.Security.MinimumPasswordLength;
                options.Password.RequiredUniqueChars = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<FutLabDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<Services.ApplicationSignInManager>()
                .AddPasswordValidator<PwnedPasswordsValidator<ApplicationUser>>();
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/login";
                options.LogoutPath = "/login/logout";
                options.AccessDeniedPath = "/login/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(this.appSettings.Security.LoginCookieExpirationDays);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FutLabDbContext dc, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) {
            // Configure database
            this.ConfigureDatabase(dc, userManager, roleManager).Wait();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Convention")]
        public async Task ConfigureDatabase(FutLabDbContext dc, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) {
            if (dc is null) throw new ArgumentNullException(nameof(dc));
            if (userManager is null) throw new ArgumentNullException(nameof(userManager));
            if (roleManager is null) throw new ArgumentNullException(nameof(roleManager));

            // Migrate database to latest version
            await dc.Database.MigrateAsync().ConfigureAwait(false);

            // Configure identity
            static void EnsureIdentitySuccess(IdentityResult result) {
                if (result == IdentityResult.Success) return;
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new Exception("Identity operation failed: " + errors);
            }

            // Create roles
            async Task EnsureRoleCreated(string roleName) {
                if (await roleManager.FindByNameAsync(roleName).ConfigureAwait(false) != null) return;
                EnsureIdentitySuccess(await roleManager.CreateAsync(new ApplicationRole { Name = roleName }).ConfigureAwait(false));
            }
            await EnsureRoleCreated(ApplicationRole.Master).ConfigureAwait(false);
            await EnsureRoleCreated(ApplicationRole.Administrator).ConfigureAwait(false);

            // Create admin user
            if (!userManager.Users.Any()) {
                var user = new ApplicationUser {
                    UserName = "Administrator",
                    Email = "michal.valasek@altairis.cz",
                    EmailConfirmed = true,
                    Language = "cs",
                    Enabled = true
                };
                EnsureIdentitySuccess(await userManager.CreateAsync(user, SecurityHelper.GenerateRandomPassword()).ConfigureAwait(false));
                EnsureIdentitySuccess(await userManager.AddToRoleAsync(user, ApplicationRole.Administrator).ConfigureAwait(false));
            }
        }

    }
}
