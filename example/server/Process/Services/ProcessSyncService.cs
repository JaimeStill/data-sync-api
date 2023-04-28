using Microsoft.AspNetCore.SignalR;
using Process.Data;
using Process.Hubs;
using Process.Models;
using Sync;

namespace Process.Services;
public class ProcessSyncService : ProcessService
{
    readonly IHubContext<ProcessHub, IProcessHub> sync;

    public ProcessSyncService(AppDbContext db, IHubContext<ProcessHub, IProcessHub> sync) : base(db)
    {
        this.sync = sync;
    }

    protected virtual string SetMessage(Package package, string action) =>
        $"Package {package.Id} - {package.Name} successfully {action}";

    protected virtual void LogAction(ISyncMessage<Package> message, string action) =>
        Console.WriteLine($"{action}: {message.Message}");

    protected override Func<Package, Task> AfterComplete => async (Package package) =>
    {
        SyncMessage<Package> message = new(
            package,
            SetMessage(package, "completed")
        );

        LogAction(message, "Complete");

        await sync
            .Clients
            .All
            .Complete(message);
    };

    protected override Func<Package, Task> AfterReceive => async (Package package) =>
    {
        SyncMessage<Package> message = new(
            package,
            SetMessage(package, "received")
        );

        LogAction(message, "Receive");

        await sync
            .Clients
            .All
            .Receive(message);
    };

    protected override Func<Package, Task> AfterReject => async (Package package) =>
    {
        SyncMessage<Package> message = new(
            package,
            SetMessage(package, "rejected")
        );

        LogAction(message, "Reject");

        await sync
            .Clients
            .All
            .Reject(message);
    };

    protected override Func<Package, Task> AfterReturn => async (Package package) =>
    {
        SyncMessage<Package> message = new(
            package,
            SetMessage(package, "returned")
        );

        LogAction(message, "Return");

        await sync
            .Clients
            .All
            .Return(message);
    };

    protected override Func<Package, Task> AfterSync => async (Package package) =>
    {
        SyncMessage<Package> message = new(
            package,
            SetMessage(package, "synced")
        );

        LogAction(message, "Sync");

        await sync
            .Clients
            .All
            .Sync(message);
    };

    protected override Func<Package, Task> AfterWithdraw => async (Package package) =>
    {
        SyncMessage<Package> message = new(
            package,
            SetMessage(package, "withdrawn")
        );

        LogAction(message, "Withdraw");

        await sync
            .Clients
            .All
            .Withdraw(message);
    };
}