using Common.Schema;

namespace Contracts.Process;
public class ResourceContract : Contract
{
    public int? PackageId { get; set; }
    public int ResourceId { get; set; }
    public string ResourceType { get; set; } = string.Empty;
    public override string Type => $"{ContractRoots.Process}Resource";

    public PackageContract? Package { get; set; }

    public override string ToString() =>
        $"{Id} - {ResourceId} - {Name} - {Type}";

    public static ResourceContract Cast<T>(T entity)
    where T : Contract => new()
    {
        Name = entity.Name,
        ResourceId = entity.Id,
        ResourceType = entity.Type
    };
}