using Process.Models;
using Sync;
using Sync.Hub;

namespace Process.Hubs;
public interface IProcessHub : ISyncHub<Package>
{
    Task Complete(ISyncMessage<Package> message);
    Task Receive(ISyncMessage<Package> message);
    Task Reject(ISyncMessage<Package> message);
    Task Return(ISyncMessage<Package> message);
    Task Withdraw(ISyncMessage<Package> message);
}