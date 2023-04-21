namespace Sync.Client;
public interface ISyncClient : IAsyncDisposable
{
    SyncClientStatus Status { get; }
    SyncAction OnCreate { get; }
    SyncAction OnUpdate { get; }
    SyncAction OnSync { get; }
    SyncAction OnDelete { get; }
    Task Connect();
    Task Join(string name);
    Task Leave(string name);
    Task Create<T>(ISyncMessage<T> message);
    Task Update<T>(ISyncMessage<T> message);
    Task Sync<T>(ISyncMessage<T> message);
    Task Delete<T>(ISyncMessage<T> message);
}