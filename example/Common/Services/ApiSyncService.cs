using Common.Schema;
using Common.Sync.Hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sync;

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

    protected virtual string SetMessage(T entity, string action) =>
        $"{typeof(T)} {entity.Name} successfully {action}";

    protected virtual void LogAction(ISyncMessage<T> message, string action) =>
        Console.WriteLine($"{action}: {message.Message}");

    protected override Func<T, Task> AfterAdd => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "created")
        );

        LogAction(message, "Add");

        await syncHub
            .Clients
            .All
            .Add(message);
    };

    protected override Func<T, Task> AfterUpdate => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "updated")
        );

        LogAction(message, "Update");

        await syncHub
            .Clients
            .All
            .Update(message);
    };

    protected override Func<T, Task> AfterRemove => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "removed")
        );

        LogAction(message, "Remove");

        await syncHub
            .Clients
            .All
            .Remove(message);
    };
}