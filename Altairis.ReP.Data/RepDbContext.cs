﻿global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace Altairis.ReP.Data;

// Abstract base class for all database contexts

public abstract class RepDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options), IDataProtectionKeyContext {

    public DbSet<CalendarEntry> CalendarEntries => this.Set<CalendarEntry>();
    public DbSet<DataProtectionKey> DataProtectionKeys => this.Set<DataProtectionKey>();
    public DbSet<DirectoryEntry> DirectoryEntries => this.Set<DirectoryEntry>();
    public DbSet<JournalEntry> JournalEntries => this.Set<JournalEntry>();
    public DbSet<JournalEntryAttachment> JournalEntryAttachments => this.Set<JournalEntryAttachment>();
    public DbSet<MaintenanceTask> MaintenanceTasks => this.Set<MaintenanceTask>();
    public DbSet<NewsMessage> NewsMessages => this.Set<NewsMessage>();
    public DbSet<OpeningHoursChange> OpeningHoursChanges => this.Set<OpeningHoursChange>();
    public DbSet<Reservation> Reservations => this.Set<Reservation>();
    public DbSet<Resource> Resources => this.Set<Resource>();
    public DbSet<ResourceAttachment> ResourceAttachments => this.Set<ResourceAttachment>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => this.Set<MaintenanceRecord>();

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().Property(x => x.Language).IsFixedLength();
        builder.Entity<ApplicationUser>().Property(x => x.ResourceAuthorizationKey).IsFixedLength();
        builder.Entity<Resource>().Property(x => x.ForegroundColor).IsFixedLength();
        builder.Entity<Resource>().Property(x => x.BackgroundColor).IsFixedLength();
    }
}

// Support for Sqlite

public class SqliteRepDbContext(DbContextOptions options) : RepDbContext(options) {
}

public class SqliteRepDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SqliteRepDbContext> {
    public SqliteRepDbContext CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<RepDbContext>();
        builder.UseSqlite("Data Source=../Altairis.ReP.Web/App_Data/ReP_design.db");
        return new SqliteRepDbContext(builder.Options);
    }
}

// Support for Microsoft SQL Server

public class SqlServerRepDbContext(DbContextOptions options) : RepDbContext(options) {
}

public class SqlServerRepDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SqlServerRepDbContext> {
    public SqlServerRepDbContext CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<RepDbContext>();
        builder.UseSqlServer("SERVER=.\\SqlExpress;TRUSTED_CONNECTION=yes;DATABASE=ReP_design");
        return new SqlServerRepDbContext(builder.Options);
    }
}