global using System.ComponentModel.DataAnnotations;
global using Altairis.ReP.Data;
global using Altairis.ReP.Web;
global using Altairis.ReP.Web.Resources;
global using Altairis.ReP.Web.Services;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
using Altairis.ConventionalMetadataProviders;
using Altairis.ReP.Web.Components;
using Altairis.Services.DateProvider;
using Altairis.Services.Mailing;
using Altairis.Services.Mailing.Rfc2822;
using Altairis.Services.Mailing.SendGrid;
using Altairis.Services.Mailing.Templating;
using Altairis.Services.PwnedPasswordsValidator;
using Altairis.SqliteBackup;
using Altairis.SqliteBackup.AzureStorage;
using Altairis.TagHelpers;
using FluentStorage;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var externalConfigPath = Path.Combine(builder.Environment.ContentRootPath, "..", "ReP.json");
builder.Configuration.AddJsonFile(externalConfigPath, optional: true);
builder.Services.Configure<AppSettings>(builder.Configuration);
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

// Configure health checks
var hcb = builder.Services.AddHealthChecks()
    .AddTypeActivatedCheck<SetupCompleteHealthCheck>(name: "Setup Complete");

// Configure database
if (appSettings.Database.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)) {
    builder.Services.AddDbContext<RepDbContext, SqlServerRepDbContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    });
    // Add health check for SQL Server
    hcb.AddSqlServer(connectionString: builder.Configuration.GetConnectionString("SqlServer") ?? throw new Exception("Connection string SqlServer is not defined."), name: "SQL Server");
} else if (appSettings.Database.Equals("Sqlite", StringComparison.OrdinalIgnoreCase)) {
    builder.Services.AddDbContext<RepDbContext, SqliteRepDbContext>(options => {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    });

    // Add health check for Sqlite
    hcb.AddSqlite(connectionString: builder.Configuration.GetConnectionString("Sqlite") ?? throw new Exception("Connection string Sqlite is not defined."), name: "Sqlite");

    // Add backup for Sqlite, if there is Azure Blob Storage configured for it
    if (!string.IsNullOrEmpty(builder.Configuration.GetConnectionString("SqliteBackupStorage"))) {
        builder.Services.AddSqliteBackup(builder.Configuration.GetConnectionString("Sqlite") ?? throw new Exception("Connection string Sqlite is not defined."))
            .WithGZip()
            .WithAzureStorageUpload(builder.Configuration.GetConnectionString("SqliteBackupStorage") ?? throw new Exception("Connection string SqliteBackupStorage is not defined."), options => {
                options.ContainerName = "sqlite-backup";
                options.CreateContainer = false;
            })
            .WithFileCleanup("*.bak.gz", 3);

        // Add health check for Sqlite backup
        builder.Services.AddSqliteBackupHealthCheck();
        hcb.AddCheck<BackupServiceHealthCheck>("SqliteBackup");
    }
} else {
    throw new Exception($"Unsupported database `{appSettings.Database}`. Use `SqlServer` or `Sqlite`.");
}

// Configure blob storage
StorageFactory.Modules.UseAzureBlobStorage();
builder.Services.AddTransient(s => StorageFactory.Blobs.FromConnectionString(builder.Configuration.GetConnectionString("Blob")));

// Configure storage of ASP.NET Data protection keys in database
builder.Services.AddDataProtection()
    .PersistKeysToDbContext<RepDbContext>();

// Configure maximum file size
builder.WebHost.ConfigureKestrel(options => {
    options.Limits.MaxRequestBodySize = appSettings.Attachments.MaximumFileSize;
});

// Configure base framework services
builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/My", "IsLoggedIn");
    options.Conventions.AuthorizeFolder("/Admin", "IsAdministrator");
    options.Conventions.AllowAnonymousToFolder("/Login");
    options.Conventions.AllowAnonymousToFolder("/Errors");
    options.Conventions.AllowAnonymousToPage("/FirstRun");
}).AddMvcOptions(options => {
    options.SetConventionalMetadataProviders<Display, Validation>();
});

// Configure identity
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsLoggedIn", policy => policy.RequireAuthenticatedUser())
    .AddPolicy("IsMaster", policy => policy.RequireRole(ApplicationRole.Master))
    .AddPolicy("IsAdministrator", policy => policy.RequireRole(ApplicationRole.Administrator));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
    options.Password.RequiredLength = appSettings.Security.MinimumPasswordLength;
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
    .AddSignInManager<ApplicationSignInManager>()
    .AddPasswordValidator<PwnedPasswordsValidator<ApplicationUser>>();
builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.Name = "ReP-Auth";
    options.LoginPath = "/login";
    options.LogoutPath = "/login/logout";
    options.AccessDeniedPath = "/login/accessdenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(appSettings.Security.LoginCookieExpirationDays);
});

// Configure mailing
if (!string.IsNullOrEmpty(appSettings.Mailing.SendGridApiKey)) {
    // Use SendGrid
    builder.Services.AddSendGridMailerService(new SendGridMailerServiceOptions {
        ApiKey = appSettings.Mailing.SendGridApiKey,
        DefaultFrom = new MailAddressDto(appSettings.Mailing.SenderAddress, appSettings.Mailing.SenderName ?? appSettings.Design.ApplicationName)
    });
} else if (!string.IsNullOrEmpty(appSettings.Mailing.SmtpHost)) {
    // Use SMTP server
    builder.Services.AddSmtpServerMailerService(new SmtpServerMailerServiceOptions {
        HostName = appSettings.Mailing.SmtpHost,
        Port = appSettings.Mailing.SmtpPort,
        AllowSsl = appSettings.Mailing.SmtpUseTls,
        UserName = appSettings.Mailing.SmtpUsername,
        Password = appSettings.Mailing.SmtpPassword,
        DefaultFrom = new MailAddressDto(appSettings.Mailing.SenderAddress, appSettings.Mailing.SenderName ?? appSettings.Design.ApplicationName)
    });
} else {
    // Use pickup folder
    builder.Services.AddPickupFolderMailerService(new PickupFolderMailerServiceOptions {
        PickupFolderName = appSettings.Mailing.PickupFolder,
        DefaultFrom = new MailAddressDto(appSettings.Mailing.SenderAddress, appSettings.Mailing.SenderName ?? appSettings.Design.ApplicationName)
    });
}
builder.Services.AddResourceTemplatedMailerService(new ResourceTemplatedMailerServiceOptions {
    ResourceType = typeof(Mailing)
});

// Configure misc services
builder.Services.AddScoped<OpeningHoursProvider>();
builder.Services.AddScoped<ResourceAttachmentProcessor>();
builder.Services.AddScoped<JournalAttachmentProcessor>();
builder.Services.AddScoped<CalendarGenerator>();
builder.Services.AddSingleton<IDateProvider>(new TzConvertDateProvider("Central Europe Standard Time", DatePrecision.Minute));
builder.Services.Configure<TimeTagHelperOptions>(options => {
    options.YesterdayDateFormatter = dt => string.Format(UI.TimeTagHelper_Yesterday, dt);
    options.TodayDateFormatter = dt => string.Format(UI.TimeTagHelper_Today, dt);
    options.TomorrowDateFormatter = dt => string.Format(UI.TimeTagHelper_Tomorrow, dt);
});

// Build application
var app = builder.Build();

// Get required services
var scope = app.Services.CreateScope();
var dc = scope.ServiceProvider.GetRequiredService<RepDbContext>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

// Migrate database to latest version
await dc.Database.MigrateAsync();

// Add ResourceAuthorizationKey to any users who don't have one (may happen in migrations)
foreach (var item in dc.Users.Where(x => x.ResourceAuthorizationKey.Equals(string.Empty))) {
    item.ResourceAuthorizationKey = ApplicationUser.CreateResourceAuthorizationKey();
}
dc.SaveChanges();

// Configure identity
static void EnsureIdentitySuccess(IdentityResult result) {
    if (result == IdentityResult.Success) return;
    var errors = string.Join(", ", result.Errors.Select(x => x.Description));
    throw new Exception("Identity operation failed: " + errors);
}

// Create roles
async Task EnsureRoleCreated(string roleName) {
    if (await roleManager.FindByNameAsync(roleName) != null) return;
    EnsureIdentitySuccess(await roleManager.CreateAsync(new ApplicationRole { Name = roleName }));
}
await EnsureRoleCreated(ApplicationRole.Master);
await EnsureRoleCreated(ApplicationRole.Administrator);

// Configure localization
var supportedCultures = LanguageSwitchViewComponent.AvailableCultures.Select(c => c.Name).ToArray();
app.UseRequestLocalization(options => {
    options.SetDefaultCulture(supportedCultures[0]);
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
});
app.MapGet("/api/set-language", LanguageSwitchViewComponent.SetCultureCookieHandler).WithName(LanguageSwitchViewComponent.SetCultureCookieHandlerRouteName);

// Configure middleware
app.UseStatusCodePagesWithReExecute("/Errors/{0}");
if (app.Environment.IsDevelopment()) {
    app.UseStaticFiles();
} else {
    app.UseStaticFiles(new StaticFileOptions {
        OnPrepareResponse = c => c.Context.Response.Headers.CacheControl = "public,max-age=31536000"
    });
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map calendar endpoints
app.MapGet("/api/full.ics", (string rak, CalendarGenerator cg) => cg.GenerateFullCalendar(rak)).WithName("FullIcs");
app.MapGet("/api/my.ics", (string rak, CalendarGenerator cg) => cg.GeneratePersonalCalendar(rak)).WithName("MyIcs");
app.MapGet("/api/{resourceId:int:min(1)}.ics", (int resourceId, string rak, CalendarGenerator cg) => cg.GenerateResourceCalendar(resourceId, rak)).WithName("ResourceIcs");

// Map other endpoints
app.MapRazorPages();
app.MapGet("/", () => Results.LocalRedirect("/My"));
app.MapHealthChecks("/api/health.json", new() {
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Run app
await app.RunAsync();