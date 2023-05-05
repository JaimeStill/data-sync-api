namespace Common.Schema;
public abstract class Contract : IContract
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public abstract string Type { get; }
    public string DateCreated { get; set; } = string.Empty;
    public string LastModified { get; set; } = string.Empty;
}