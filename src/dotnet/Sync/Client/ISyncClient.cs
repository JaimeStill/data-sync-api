namespace Sync.Client;
public interface ISyncClient : IAsyncDisposable
{
    SyncClientStatus Status { get; }
    SyncAction OnAdd { get; }
    SyncAction OnUpdate { get; }
    SyncAction OnSync { get; }
    SyncAction OnRemove { get; }
    Task Connect(CancellationToken token);
    Task Join(string name);
    Task Leave(string name);
    Task Add<T>(ISyncMessage<T> message);
    Task Update<T>(ISyncMessage<T> message);
    Task Sync<T>(ISyncMessage<T> message);
    Task Remove<T>(ISyncMessage<T> message);
}