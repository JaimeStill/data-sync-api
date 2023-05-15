using Sync.Client;

namespace Common.Sync.Client;
public interface IApiSyncClient<T> : ISyncClient<T>
{
    SyncAction OnAdd { get; }
    SyncAction OnUpdate { get; }    
    SyncAction OnRemove { get; }
}