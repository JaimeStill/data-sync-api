using System.Reflection;
using Common.Db;
using Microsoft.EntityFrameworkCore;
using Process.Models;

namespace Process.Data;
public class AppDbContext : EntityContext<AppDbContext>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Resource> Resources => Set<Resource>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly()
        );
    }
}