using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Sync;
using Sync.Client;

namespace Contracts.Process;
public class ProcessSync : SyncClient<PackageContract>, IProcessSync
{
    public SyncAction OnComplete { get; protected set; }
    public SyncAction OnReceive { get; protected set; }
    public SyncAction OnReject { get; protected set; }
    public SyncAction OnReturn { get; protected set; }
    public SyncAction OnWithdraw { get; protected set; }

    static void WriteMessage(SyncMessage<PackageContract> message) =>
        Console.WriteLine(message.Message);

    void Initialize()
    {
        OnComplete.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnReceive.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnReject.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnReturn.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnSync.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnWithdraw.Set<SyncMessage<PackageContract>>(WriteMessage);
    }

    public ProcessSync(IConfiguration config) : base(
        config.GetValue<string>("Sync:Process") ?? "http://localhost:5002/sync/process/"
    )
    {
        InitializeActions();
        Initialize();
    }

    public ProcessSync(string endpoint) : base(endpoint)
    {
        InitializeActions();
        Initialize();
    }

    [MemberNotNull(
        nameof(OnComplete),
        nameof(OnReceive),
        nameof(OnReject),
        nameof(OnReturn),
        nameof(OnWithdraw)
    )]
    protected override void InitializeActions()
    {
        base.InitializeActions();

        OnComplete = new("Complete", connection);
        OnReceive = new("Receive", connection);
        OnReject = new("Reject", connection);
        OnReturn = new("Return", connection);
        OnWithdraw = new("Withdraw", connection);
    }

    protected override void DisposeEvents()
    {
        base.DisposeEvents();
        OnComplete.Dispose();
        OnReceive.Dispose();
        OnReject.Dispose();
        OnReturn.Dispose();
        OnWithdraw.Dispose();
    }
}