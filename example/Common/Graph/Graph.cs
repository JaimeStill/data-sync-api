namespace Common.Graph;
public record Graph
{
    public Guid Id { get; set; }
    public List<Endpoint> Endpoints { get; set; } = new();
}