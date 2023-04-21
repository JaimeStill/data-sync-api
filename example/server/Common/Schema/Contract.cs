namespace Common.Schema;
public abstract class Contract
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DateCreated { get; set; } = string.Empty;
    public string LastModified { get; set; } = string.Empty;
}