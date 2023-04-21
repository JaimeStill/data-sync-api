using Common.Schema;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sync;
using Sync.Hub;

namespace Common.Services;
public abstract class SyncService<T, S, Db> : EntityService<T, Db>
     where T : Entity
     where S : SyncHub<IContract>
     where Db : DbContext
{
    protected IHubContext<S, ISyncHub<IContract>> sync;
    protected string channel;
    public SyncService(Db db, IHubContext<S, ISyncHub<IContract>> sync) : base(db)
    {
        this.sync = sync;
        channel = $"{typeof(T)}".ToLower();
        Console.WriteLine($"Channel: {channel}");
    }

    protected override Func<T, Task> AfterAdd => async (T entity) =>
    {
        SyncMessage<IContract> message = new(
            channel,
            entity,
            ActionType.Add,
            $"{typeof(T)} successfully created"
        );

        await sync
            .Clients
            .Group(channel)
            .Add(message);
    };

    protected override Func<T, Task> AfterUpdate => async (T entity) =>
    {
        SyncMessage<IContract> message = new(
            channel,
            entity,
            ActionType.Update,
            $"{typeof(T)} successfully updated"
        );

        await sync
            .Clients
            .Group(channel)
            .Update(message);
    };

    protected override Func<T, Task> AfterRemove => async (T entity) =>
    {
        SyncMessage<IContract> message = new(
            channel,
            entity,
            ActionType.Update,
            $"{typeof(T)} successfully updated"
        );

        await sync
            .Clients
            .Group(channel)
            .Remove(message);
    };
}