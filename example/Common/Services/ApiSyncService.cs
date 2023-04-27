using Common.Schema;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sync;
using Sync.Hub;

namespace Common.Services;
public abstract class ApiSyncService<T, H, Db> : EntityService<T, Db>
    where T : Entity
    where H : ApiSyncHub<T>
    where Db : DbContext
{
    protected IHubContext<H, IApiSyncHub<T>> syncHub;
    public ApiSyncService(Db db, IHubContext<H, IApiSyncHub<T>> sync) : base(db)
    {
        syncHub = sync;
    }

    protected override Func<T, Task> AfterAdd => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            $"{typeof(T)} successfully created"
        );

        await syncHub
            .Clients
            .All
            .Add(message);
    };

    protected override Func<T, Task> AfterUpdate => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            $"{typeof(T)} successfully updated"
        );

        await syncHub
            .Clients
            .All
            .Update(message);
    };

    protected override Func<T, Task> AfterRemove => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            $"{typeof(T)} successfully removed"
        );

        await syncHub
            .Clients
            .All
            .Remove(message);
    };
}