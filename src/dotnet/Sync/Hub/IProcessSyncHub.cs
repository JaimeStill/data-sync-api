namespace Sync.Hub;
public interface IProcessSyncHub<T> : ISyncHub<T>
{
    Task Complete(ISyncMessage<T> message);
    Task Receive(ISyncMessage<T> message);
    Task Reject(ISyncMessage<T> message);
    Task Return(ISyncMessage<T> message);
    Task Withdraw(ISyncMessage<T> message);
}