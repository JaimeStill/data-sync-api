using Common.Schema;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sync;
using Sync.Hub;

namespace Common.Services;
public abstract class SyncService<T, H, Db> : EntityService<T, Db>
     where T : Entity
     where H : SyncHub<T>
     where Db : DbContext
{
    protected IHubContext<H, ISyncHub<T>> sync;
    public SyncService(Db db, IHubContext<H, ISyncHub<T>> sync) : base(db)
    {
        this.sync = sync;
    }

    protected override Func<T, Task> AfterAdd => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            ActionType.Add,
            $"{typeof(T)} successfully created"
        );

        await sync
            .Clients
            .All
            .Add(message);
    };

    protected override Func<T, Task> AfterUpdate => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            ActionType.Update,
            $"{typeof(T)} successfully updated"
        );

        await sync
            .Clients
            .All
            .Update(message);
    };

    protected override Func<T, Task> AfterRemove => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            ActionType.Update,
            $"{typeof(T)} successfully updated"
        );

        await sync
            .Clients
            .All
            .Remove(message);
    };
}