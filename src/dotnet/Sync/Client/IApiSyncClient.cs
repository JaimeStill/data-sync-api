namespace Sync.Client;
public interface IApiSyncClient<T> : ISyncClient<T>
{
    SyncAction OnAdd { get; }
    SyncAction OnUpdate { get; }    
    SyncAction OnRemove { get; }
    Task Add(ISyncMessage<T> message);
    Task Update(ISyncMessage<T> message);
    Task Remove(ISyncMessage<T> message);
}