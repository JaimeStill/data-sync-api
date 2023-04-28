using Common.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Common.Db;
public abstract class EntityContext<T> : DbContext where T : DbContext
{
    public EntityContext(DbContextOptions<T> options) : base(options)
    {
        SavingChanges += OnSaving;
    }

    protected IEnumerable<EntityEntry> ChangeTrackerEntities() =>
        ChangeTracker
            .Entries()
            .Where(x => x.Entity is Entity);

    protected bool EntitiesChanged() =>
        ChangeTrackerEntities().Any();

    protected virtual void OnSaving(object? sender, SavingChangesEventArgs e)
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