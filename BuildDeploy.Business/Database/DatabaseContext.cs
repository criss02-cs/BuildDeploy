﻿using BuildDeploy.Business.Entity;
using Microsoft.EntityFrameworkCore;

namespace BuildDeploy.Business.Database;
public class DatabaseContext : DbContext
{
    private static DatabaseContext? _instance;
    public static DatabaseContext Instance => _instance ??= new DatabaseContext();

    public DbSet<Project> Projects { get; set; }
    public DbSet<FtpProfile> FtpProfiles { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = BuildDeploy.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasOne(x => x.FtpProfile)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.FtpProfileId);
        modelBuilder.Entity<FtpProfile>()
            .HasMany(x => x.Projects)
            .WithOne(x => x.FtpProfile);
    }
}
