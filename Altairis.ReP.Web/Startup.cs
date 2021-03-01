using System;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ConventionalMetadataProviders;
using Altairis.ReP.Data;
using Altairis.ReP.Web.Resources;
using Altairis.ReP.Web.Services;
using Altairis.Services.DateProvider;
using Altairis.Services.Mailing;
using Altairis.Services.Mailing.Rfc2822;
using Altairis.Services.Mailing.SendGrid;
using Altairis.Services.Mailing.Templating;
using Altairis.Services.PwnedPasswordsValidator;
using Altairis.TagHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Altairis.ReP.Web {
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
            // Configure database
            services.AddDbContext<RepDbContext>(options => {
                options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection"));
            });

            // Configure base framework services
            services.AddRazorPages(options => {
                options.Conventions.AuthorizeFolder("/My", "IsLoggedIn");
                options.Conventions.AuthorizeFolder("/Admin", "IsAdministrator");
                options.Conventions.AllowAnonymousToFolder("/Login");
                options.Conventions.AllowAnonymousToFolder("/Errors");
                options.Conventions.AllowAnonymousToPage("/FirstRun");
            }).AddMvcOptions(options => {
                options.SetConventionalMetadataProviders<Display, Validation>();
            });

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
                .AddEntityFrameworkStores<RepDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<Services.ApplicationSignInManager>()
                .AddPasswordValidator<PwnedPasswordsValidator<ApplicationUser>>();
            services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = "ReP-Auth";
                options.LoginPath = "/login";
                options.LogoutPath = "/login/logout";
                options.AccessDeniedPath = "/login/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(this.appSettings.Security.LoginCookieExpirationDays);
            });

            // Configure mailing
            if (this.appSettings.Mailing.UseSendGrid && !string.IsNullOrEmpty(this.appSettings.Mailing.SendGridApiKey)) {
                services.AddSendGridMailerService(new SendGridMailerServiceOptions {
                    ApiKey = this.appSettings.Mailing.SendGridApiKey,
                    DefaultFrom = new MailAddressDto(this.appSettings.Mailing.SenderAddress, this.appSettings.Mailing.SenderName)
                });
            } else {
                services.AddPickupFolderMailerService(new PickupFolderMailerServiceOptions {
                    PickupFolderName = this.appSettings.Mailing.PickupFolder,
                    DefaultFrom = new MailAddressDto(this.appSettings.Mailing.SenderAddress, this.appSettings.Mailing.SenderName)
                });
            }
            services.AddResourceTemplatedMailerService(new ResourceTemplatedMailerServiceOptions {
                ResourceType = typeof(Mailing)
            });

            // Configure misc services
            services.Configure<AppSettings>(this.configuration);
            services.AddSingleton<IDateProvider>(new TzConvertDateProvider("Central Europe Standard Time", DatePrecision.Minute));
            services.AddScoped<OpeningHoursProvider>();
            services.Configure<TimeTagHelperOptions>(options => {
                options.YesterdayDateFormatter = dt => string.Format(UI.TimeTagHelper_Yesterday, dt);
                options.TodayDateFormatter = dt => string.Format(UI.TimeTagHelper_Today, dt);
                options.TomorrowDateFormatter = dt => string.Format(UI.TimeTagHelper_Tomorrow, dt);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RepDbContext dc, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) {
            // Configure database
            this.ConfigureDatabase(dc, userManager, roleManager).Wait();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // Configure localization
            var supportedCultures = new[] { "cs-CZ", "en-US" };
            app.UseRequestLocalization(options => {
                options.SetDefaultCulture(supportedCultures[0]);
                options.AddSupportedCultures(supportedCultures);
                options.AddSupportedUICultures(supportedCultures);
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
            });

            // Configure middleware
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Convention")]
        public async Task ConfigureDatabase(RepDbContext dc, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) {
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
        }

    }
}
