using System.Reflection;
using System.Text.Json;
using BuildDeploy.Business.Entity;
using Microsoft.EntityFrameworkCore;

namespace BuildDeploy.Business.Database;
public class DatabaseContext : DbContext
{
    private static DatabaseContext? _instance;
    public static DatabaseContext Instance => _instance ??= new DatabaseContext();

    public DbSet<Project> Projects { get; set; }
    public DbSet<FtpProfile> FtpProfiles { get; set; }
    public DbSet<Language> Languages { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // usa la path di AppData del pc
        var path = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "BuildDeploy");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        } 
        optionsBuilder.UseSqlite($"Data Source = {Path.Combine(path, "BuildDeploy.db")}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasOne(x => x.FtpProfile)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.FtpProfileId);
        modelBuilder.Entity<Project>()
            .HasOne(x => x.Language)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.LanguageId);
        modelBuilder.Entity<FtpProfile>()
            .HasMany(x => x.Projects)
            .WithOne(x => x.FtpProfile);
        modelBuilder.Entity<Language>()
            .HasMany(x => x.Projects)
            .WithOne(x => x.Language);
        modelBuilder.Entity<Language>()
            .Ignore(x => x.Arguments);
    }
}
