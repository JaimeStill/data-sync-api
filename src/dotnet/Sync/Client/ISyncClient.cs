namespace Sync.Client;
public interface ISyncClient<T> : IAsyncDisposable
{
    SyncClientStatus Status { get; }
    SyncAction OnSync { get; }
    Task Connect(CancellationToken token);
    Task Sync(ISyncMessage<T> message);
}