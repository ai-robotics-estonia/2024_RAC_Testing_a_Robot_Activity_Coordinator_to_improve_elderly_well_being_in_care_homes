using App.Domain;
using App.Domain.Attendance;
using App.Domain.Identity;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IDataProtectionKeyContext
{
    public DbSet<Lecture> Lectures { get; set; } = default!;
    public DbSet<LectureAttendance> LectureAttendances { get; set; } = default!;
    
    public DbSet<Domain.App> Apps { get; set; } = default!;
    public DbSet<AppVersion> AppVersions { get; set; } = default!;
    public DbSet<Article> Articles { get; set; } = default!;
    public DbSet<ArticleCategory> ArticleCategories { get; set; } = default!;
    public DbSet<GreetingPhrase> GreetingPhrases { get; set; } = default!;
    public DbSet<GreetingPhraseCategory> GreetingPhraseCategories { get; set; } = default!;
    public DbSet<LogEvent> LogEvents { get; set; } = default!;
    public DbSet<Map> Maps { get; set; } = default!;
    public DbSet<MapFloor> MapFloors { get; set; } = default!;
    public DbSet<MapLocation> MapLocations { get; set; } = default!;
    public DbSet<Organization> Organizations { get; set; } = default!;
    public DbSet<OrganizationAppUser> OrganizationAppUsers { get; set; } = default!;
    public DbSet<Robot> Robots { get; set; } = default!;
    public DbSet<RobotMapApp> RobotMapApps { get; set; } = default!;
    public DbSet<RobotMapAppOrganization> RobotMapAppOrganizations { get; set; } = default!;
    public DbSet<WebLink> WebLinks { get; set; } = default!;

    public DbSet<WebLinkCategory> WebLinkCategories { get; set; } = default!;

    // This maps to the table that stores data protection keys.
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // disable cascade delete globally
        foreach (var relationship in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // enable cascade delete for specific relationships
        // RobotMapApp -> Article
        builder.Entity<RobotMapApp>()
            .HasMany(e => e.Articles)
            .WithOne(e => e.RobotMapApp)
            .OnDelete(DeleteBehavior.Cascade);

        // RobotMapApp -> LogEvent
        builder.Entity<RobotMapApp>()
            .HasMany(e => e.LogEvents)
            .WithOne(e => e.RobotMapApp)
            .OnDelete(DeleteBehavior.Cascade);

        // MapFloor -> MapLocation
        builder.Entity<MapFloor>()
            .HasMany(f => f.MapLocations)
            .WithOne(l => l.MapFloor)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WebLink>()
            .Property(w => w.WebLinkDisplayName)
            .HasDefaultValueSql("'{}'::jsonb");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // for postgres, change all datetime types to UTC
        foreach (var entity in ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted))
        {
            foreach (var prop in entity
                         .Properties
                         .Where(x => x.Metadata.ClrType == typeof(DateTime)))
            {
                prop.CurrentValue = ((DateTime)prop.CurrentValue!).ToUniversalTime();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}