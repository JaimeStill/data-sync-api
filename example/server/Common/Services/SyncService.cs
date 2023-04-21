using Common.Schema;
using Microsoft.EntityFrameworkCore;
using Sync;
using Sync.Client;

namespace Common.Services;
public abstract class SyncService<T, Db> : EntityService<T, Db>
     where T : Entity
     where Db : DbContext
{
    protected ISyncClient sync;
    protected string channel;
    public SyncService(Db db, ISyncClient sync) : base(db)
    {
        this.sync = sync;
        channel = $"{typeof(T)}".ToLower();
    }

    protected override Func<T, Task> AfterAdd => async (T entity) =>
        await sync.Add(new SyncMessage<T>(
            channel,
            entity,
            ActionType.Add,
            $"{typeof(T)} successfully created"
        ));

    protected override Func<T, Task> AfterUpdate => async (T entity) =>
        await sync.Update(new SyncMessage<T>(
            channel,
            entity,
            ActionType.Update,
            $"{typeof(T)} successfully updated"
        ));

    protected override Func<T, Task> AfterRemove => async (T entity) =>
        await sync.Remove(new SyncMessage<T>(
            channel,
            entity,
            ActionType.Update,
            $"{typeof(T)} successfully updated"
        ));
}