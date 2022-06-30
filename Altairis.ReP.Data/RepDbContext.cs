global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;

namespace Altairis.ReP.Data;

public class RepDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int> {
    public RepDbContext(DbContextOptions options) : base(options) {
    }

    public DbSet<Resource> Resources => this.Set<Resource>();

    public DbSet<Reservation> Reservations => this.Set<Reservation>();

    public DbSet<OpeningHoursChange> OpeningHoursChanges => this.Set<OpeningHoursChange>();

    public DbSet<NewsMessage> NewsMessages => this.Set<NewsMessage>();

    public DbSet<DirectoryEntry> DirectoryEntries => this.Set<DirectoryEntry>();

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().Property(x => x.Language).IsFixedLength();
        builder.Entity<Resource>().Property(x => x.ForegroundColor).IsFixedLength();
        builder.Entity<Resource>().Property(x => x.BackgroundColor).IsFixedLength();
    }
}

public class FutLabDbContextDesignTimeFactory : IDesignTimeDbContextFactory<RepDbContext> {
    public RepDbContext CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<RepDbContext>();
        builder.UseSqlServer("SERVER=.\\SqlExpress;TRUSTED_CONNECTION=yes;DATABASE=ReP_design");
        return new RepDbContext(builder.Options);
    }
}
