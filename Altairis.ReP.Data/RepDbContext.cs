using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Altairis.ReP.Data;
public class RepDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int> {
    public RepDbContext(DbContextOptions options) : base(options) {
    }

    public DbSet<Resource> Resources { get; set; }

    public DbSet<Reservation> Reservations { get; set; }

    public DbSet<OpeningHoursChange> OpeningHoursChanges { get; set; }

    public DbSet<NewsMessage> NewsMessages { get; set; }

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
