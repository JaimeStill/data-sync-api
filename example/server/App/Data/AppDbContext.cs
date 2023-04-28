using App.Models;
using Common.Db;
using Microsoft.EntityFrameworkCore;

namespace App.Data;
public class AppDbContext : EntityContext<AppDbContext>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Proposal> Proposals => Set<Proposal>();
}