using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore;

public class RepDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public RepDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Resource> Resources => Set<Resource>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<OpeningHoursChange> OpeningHoursChanges => Set<OpeningHoursChange>();

    public DbSet<NewsMessage> NewsMessages => Set<NewsMessage>();

    public DbSet<DirectoryEntry> DirectoryEntries => Set<DirectoryEntry>();

    public DbSet<CalendarEntry> CalendarEntries => Set<CalendarEntry>();

    public DbSet<ResourceAttachment> ResourceAttachments => Set<ResourceAttachment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().Property(x => x.Language).IsFixedLength();
        builder.Entity<Resource>().Property(x => x.ForegroundColor).IsFixedLength();
        builder.Entity<Resource>().Property(x => x.BackgroundColor).IsFixedLength();
        builder.Entity<OpeningHoursChange>().HasIndex(x => x.Date).IsUnique();
    }
}

public class RepDbContextDesignTimeFactory : IDesignTimeDbContextFactory<RepDbContext>
{
    public RepDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<RepDbContext>();
        // builder.UseSqlServer("SERVER=.\\SqlExpress;TRUSTED_CONNECTION=yes;DATABASE=ReP_design");
        builder.UseSqlServer("SERVER=.;TRUSTED_CONNECTION=yes;DATABASE=ReP_design");
        return new RepDbContext(builder.Options);
    }
}
