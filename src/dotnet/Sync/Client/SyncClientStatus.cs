namespace Sync.Client;
public class SyncClientStatus
{
    public string? ConnectionId { get; set; }
    public string State { get; set; }

    public SyncClientStatus(string? connectionId, string state)
    {
        ConnectionId = connectionId;
        State = state;
    }
}