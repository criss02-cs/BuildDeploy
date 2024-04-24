using BuildDeploy.Business.Entity;
using Microsoft.EntityFrameworkCore;

namespace BuildDeploy.Business.Database;
public class DatabaseContext : DbContext
{
    private static DatabaseContext? _instance;
    public static DatabaseContext Instance => _instance ??= new DatabaseContext();

    public DbSet<Project> Projects { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = BuildDeploy.db");
    }
}
