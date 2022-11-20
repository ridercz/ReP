global using Altairis.ReP.Web;
global using Altairis.ReP.Web.Resources;
global using Altairis.ReP.Web.Services;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.EntityFrameworkCore;
global using Olbrasoft.Data.Cqrs;
global using Olbrasoft.ReP.Business.Abstractions;
global using System.ComponentModel.DataAnnotations;
using Altairis.ConventionalMetadataProviders;
using Altairis.ReP.Data.Entities;
using Altairis.Services.DateProvider;
using Altairis.Services.Mailing;
using Altairis.Services.Mailing.Rfc2822;
using Altairis.Services.Mailing.SendGrid;
using Altairis.Services.Mailing.Templating;
using Altairis.Services.PwnedPasswordsValidator;
using Altairis.TagHelpers;
using IGeekFan.AspNetCore.Identity.FreeSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Olbrasoft.Blog.Data.MappingRegisters;
using Olbrasoft.Data.Cqrs.FreeSql;
using Olbrasoft.Extensions.DependencyInjection;
using Olbrasoft.Mapping.Mapster.DependencyInjection.Microsoft;
using Olbrasoft.ReP.Business;
using Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore;
using Olbrasoft.ReP.Data.Cqrs.FreeSql;
using Storage.Net;

var builder = WebApplication.CreateBuilder(args);


// Load configuration
var externalConfigPath = Path.Combine(builder.Environment.ContentRootPath, "..", "ReP.json");
builder.Configuration.AddJsonFile(externalConfigPath, optional: true);
builder.Services.Configure<AppSettings>(builder.Configuration);
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

IFreeSql fsql = new FreeSql.FreeSqlBuilder()
      .UseConnectionString(FreeSql.DataType.SqlServer, builder.Configuration.GetConnectionString("SqlServer"))
      .UseAutoSyncStructure(false) //automatically synchronize the entity structure to the database
      .Build();

builder.Services.AddSingleton(fsql);

builder.Services.AddFreeDbContext<RepDbContextFreeSql>(o =>
{
    o.UseFreeSql(fsql);
});


// Configure database
if (appSettings.Database.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<RepDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    });

}
else if (appSettings.Database.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<RepDbContext, SqliteRepDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    });
}
else
{
    throw new Exception($"Unsupported database `{appSettings.Database}`. Use `SqlServer` or `Sqlite`.");
}

//FreeSql projection
//builder.Services.AddProjectionConfigurations(typeof(RepDbContextFreeSql).Assembly);


//Cqrs urèuje které handlery respektivnì jestli EF nebo FreeSql ještì je potøeba pøepnout
//.AddFreeSqlStores<RepDbContextFreeSql>()/ AddEntityFrameworkStores<RepDbContext>
// u addidentity
builder.Services.AddDispatching(typeof(RepDbContext).Assembly);


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = appSettings.Security.MinimumPasswordLength;
    options.Password.RequiredUniqueChars = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
})//.AddFreeSqlStores<RepDbContextFreeSql>()
     .AddEntityFrameworkStores<RepDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<Altairis.ReP.Web.Services.ApplicationSignInManager>()
    .AddPasswordValidator<PwnedPasswordsValidator<ApplicationUser>>();



// Configure blob storage
StorageFactory.Modules.UseAzureBlobStorage();
builder.Services.AddTransient(s => StorageFactory.Blobs.FromConnectionString(builder.Configuration.GetConnectionString("Blob")));

// Configure maximum file size
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = appSettings.Attachments.MaximumFileSize;
});

// Configure base framework services
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/My", "IsLoggedIn");
    options.Conventions.AuthorizeFolder("/Admin", "IsAdministrator");
    options.Conventions.AllowAnonymousToFolder("/Login");
    options.Conventions.AllowAnonymousToFolder("/Errors");
    options.Conventions.AllowAnonymousToPage("/FirstRun");
}).AddMvcOptions(options =>
{
    options.SetConventionalMetadataProviders<Display, Validation>();
});

// Configure identity
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsLoggedIn", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("IsMaster", policy => policy.RequireRole(ApplicationRole.Master));
    options.AddPolicy("IsAdministrator", policy => policy.RequireRole(ApplicationRole.Administrator));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "ReP-Auth";
    options.LoginPath = "/login";
    options.LogoutPath = "/login/logout";
    options.AccessDeniedPath = "/login/accessdenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(appSettings.Security.LoginCookieExpirationDays);
});

// Configure mailing
if (!string.IsNullOrEmpty(appSettings.Mailing.SendGridApiKey))
{
    builder.Services.AddSendGridMailerService(new SendGridMailerServiceOptions
    {
        ApiKey = appSettings.Mailing.SendGridApiKey,
        DefaultFrom = new MailAddressDto(appSettings.Mailing.SenderAddress, appSettings.Mailing.SenderName ?? appSettings.Design.ApplicationName)
    });
}
else
{
    builder.Services.AddPickupFolderMailerService(new PickupFolderMailerServiceOptions
    {
        PickupFolderName = appSettings.Mailing.PickupFolder,
        DefaultFrom = new MailAddressDto(appSettings.Mailing.SenderAddress, appSettings.Mailing.SenderName ?? appSettings.Design.ApplicationName)
    });
}
builder.Services.AddResourceTemplatedMailerService(new ResourceTemplatedMailerServiceOptions
{
    ResourceType = typeof(Mailing)
});

// Configure misc services
builder.Services.AddSingleton<IDateProvider>(new TzConvertDateProvider("Central Europe Standard Time", DatePrecision.Minute));
builder.Services.AddScoped<OpeningHoursProvider>();
builder.Services.AddScoped<AttachmentProcessor>();
builder.Services.Configure<TimeTagHelperOptions>(options =>
{
    options.YesterdayDateFormatter = dt => string.Format(UI.TimeTagHelper_Yesterday, dt);
    options.TodayDateFormatter = dt => string.Format(UI.TimeTagHelper_Today, dt);
    options.TomorrowDateFormatter = dt => string.Format(UI.TimeTagHelper_Tomorrow, dt);
});


//Mapper
builder.Services.AddMapping(typeof(ReservationTo_ConflictDtoRegister).Assembly);


//business
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<INewsMessageService, NewsMessageService>();
builder.Services.AddScoped<ICalendarEntryService, CalendarEntryService>();
builder.Services.AddScoped<IDirectoryEntryService, DirectoryEntryService>();
builder.Services.AddScoped<IResourceAttachmentService, ResourceAttachmentService>();
builder.Services.AddScoped<IOpeningHoursChangeService, OpeningHoursChangeService>();
builder.Services.AddScoped<IUserService, UserService>();


// Build application
var app = builder.Build();

// Get required services
var scope = app.Services.CreateScope();
//var dc = scope.ServiceProvider.GetRequiredService<RepDbContext>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

// Migrate database to latest version
//await dc.Database.MigrateAsync().ConfigureAwait(false);

// Configure identity
static void EnsureIdentitySuccess(IdentityResult result)
{
    if (result == IdentityResult.Success) return;
    var errors = string.Join(", ", result.Errors.Select(x => x.Description));
    throw new Exception("Identity operation failed: " + errors);
}

// Create roles
async Task EnsureRoleCreated(string roleName)
{
    if (await roleManager.FindByNameAsync(roleName).ConfigureAwait(false) != null) return;
    EnsureIdentitySuccess(await roleManager.CreateAsync(new ApplicationRole { Name = roleName }).ConfigureAwait(false));
}
await EnsureRoleCreated(ApplicationRole.Master).ConfigureAwait(false);
await EnsureRoleCreated(ApplicationRole.Administrator).ConfigureAwait(false);

// Configure localization
var supportedCultures = new[] { "cs-CZ", "en-US" };
app.UseRequestLocalization(options =>
{
    options.SetDefaultCulture(supportedCultures[0]);
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
});

// Configure middleware
app.UseStatusCodePagesWithReExecute("/Errors/{0}");
if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
}
else
{
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = c => c.Context.Response.Headers.Add("Cache-Control", "public,max-age=31536000")
    });
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapRazorPages();

// Run app
await app.RunAsync();