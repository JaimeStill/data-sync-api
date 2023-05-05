using System.Diagnostics.CodeAnalysis;

namespace Sync.Client;
public abstract class ProcessSyncClient<T> : SyncClient<T>, IProcessSyncClient<T>
{
    public SyncAction OnComplete { get; protected set; }
    public SyncAction OnReceive { get; protected set; }
    public SyncAction OnReject { get; protected set; }
    public SyncAction OnReturn { get; protected set; }
    public SyncAction OnWithdraw { get; protected set; }

    [MemberNotNull(
        nameof(OnComplete),
        nameof(OnReceive),
        nameof(OnReject),
        nameof(OnReturn),
        nameof(OnWithdraw)
    )]
    void Initialize()
    {
        OnComplete = new("Complete", connection);
        OnReceive = new("Receive", connection);
        OnReject = new("Reject", connection);
        OnReturn = new("Return", connection);
        OnWithdraw = new("Withdraw", connection);
    }

    public ProcessSyncClient(string endpoint) : base(endpoint)
    {
        Initialize();
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