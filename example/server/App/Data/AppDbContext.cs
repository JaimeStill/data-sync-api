using App.Models;
using Common.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        SavingChanges += CompleteEntity;
    }

    public DbSet<Proposal> Proposals => Set<Proposal>();

    private IEnumerable<EntityEntry> ChangeTrackerEntities() =>
        ChangeTracker
            .Entries()
            .Where(x => x.Entity is Entity);

    private bool EntitiesChanged() =>
        ChangeTrackerEntities().Any();

    private void CompleteEntity(object? sender, SavingChangesEventArgs e)
    {
        if (EntitiesChanged())
        {
            IEnumerable<Entity>? entities = ChangeTrackerEntities()
                .Select(x => x.Entity)
                .Cast<Entity>();

            foreach (Entity entity in entities)
                entity.OnSaving();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Model
            .GetEntityTypes()
            .Where(x => x.BaseType == null)
            .ToList()
            .ForEach(x =>
                modelBuilder
                    .Entity(x.Name)
                    .ToTable(x.DisplayName())
            );
    }
}