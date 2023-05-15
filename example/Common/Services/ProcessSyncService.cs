using Common.Schema;
using Common.Sync.Hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sync;

namespace Common.Services;
public abstract class ProcessSyncService<T, H, Db> : ProcessService<T, Db>
    where T : Entity
    where H : ProcessSyncHub<T>
    where Db : DbContext
{
    protected IHubContext<H, IProcessSyncHub<T>> syncHub;
    public ProcessSyncService(Db db, IHubContext<H, IProcessSyncHub<T>> sync) : base(db)
    {
        syncHub = sync;
    }

    protected virtual string SetMessage(T entity, string action) =>
        $"{typeof(T)} {entity.Name} successfully {action}";

    protected virtual void LogAction(ISyncMessage<T> message, string action) =>
        Console.WriteLine($"{action}: {message.Message}");

    protected override Func<T, Task> AfterComplete => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "completed")
        );

        LogAction(message, "Complete");

        await syncHub
            .Clients
            .All
            .Complete(message);
    };

    protected override Func<T, Task> AfterReceive => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "received")
        );

        LogAction(message, "Receive");

        await syncHub
            .Clients
            .All
            .Receive(message);
    };

    protected override Func<T, Task> AfterReject => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "rejected")
        );

        LogAction(message, "Reject");

        await syncHub
            .Clients
            .All
            .Reject(message);
    };

    protected override Func<T, Task> AfterReturn => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "returned")
        );

        LogAction(message, "Return");

        await syncHub
            .Clients
            .All
            .Return(message);
    };

    protected override Func<T, Task> AfterSync => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "synced")
        );

        LogAction(message, "Sync");

        await syncHub
            .Clients
            .All
            .Sync(message);
    };

    protected override Func<T, Task> AfterWithdraw => async (T entity) =>
    {
        SyncMessage<T> message = new(
            entity,
            SetMessage(entity, "withdrawn")
        );

        LogAction(message, "Withdraw");

        await syncHub
            .Clients
            .All
            .Withdraw(message);
    };
}