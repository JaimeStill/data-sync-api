namespace Sync;
public class Channel
{
    public string Name { get; private set; }
    public List<string> Connections { get; private set; }

    public Channel(string name, List<string> connections)
    {
        Name = name;
        Connections = connections;
    }
}