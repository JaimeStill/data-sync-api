using Sync.Client;

namespace Contracts.Process;
public interface IProcessSync : ISyncClient<PackageContract>
{
    SyncAction OnComplete { get; }
    SyncAction OnReceive { get; }
    SyncAction OnReject { get; }
    SyncAction OnReturn { get; }
    SyncAction OnWithdraw { get; }
}