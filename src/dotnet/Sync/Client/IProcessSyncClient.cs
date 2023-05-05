namespace Sync.Client;
public interface IProcessSyncClient<T> : ISyncClient<T>
{
    SyncAction OnComplete { get; }
    SyncAction OnReceive { get; }
    SyncAction OnReject { get; }
    SyncAction OnReturn { get; }
    SyncAction OnWithdraw { get; }
}