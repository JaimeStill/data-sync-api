using Microsoft.AspNetCore.SignalR.Client;

namespace Sync.Client;
public class SyncClientStatus
{
    public string? ConnectionId { get; set; }
    public HubConnectionState State { get; set; }

    public SyncClientStatus(string? connectionId, HubConnectionState state)
    {
        ConnectionId = connectionId;
        State = state;
    }
}