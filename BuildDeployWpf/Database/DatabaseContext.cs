using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeployWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildDeployWpf.Database;
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
