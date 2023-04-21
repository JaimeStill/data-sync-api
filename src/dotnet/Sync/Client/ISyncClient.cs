namespace Sync.Client;
public interface ISyncClient<T> : IAsyncDisposable
{
    SyncClientStatus Status { get; }
    SyncAction OnAdd { get; }
    SyncAction OnUpdate { get; }
    SyncAction OnSync { get; }
    SyncAction OnRemove { get; }
    Task Connect(CancellationToken token);
    Task Join(string name);
    Task Leave(string name);
    Task Add(ISyncMessage<T> message);
    Task Update(ISyncMessage<T> message);
    Task Sync(ISyncMessage<T> message);
    Task Remove(ISyncMessage<T> message);
}