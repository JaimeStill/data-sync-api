using Common.Schema;

namespace Contracts.Process;
public class ResourceContract : Contract
{
    public int? PackageId { get; set; }
    public int ResourceId { get; set; }
    public string Type { get; set; } = "Type";

    public PackageContract? Package { get; set; }

    public override string ToString() =>
        $"{Id} - {ResourceId} - {Name} - {Type}";

    public static ResourceContract Cast<T>(T entity)
    where T : Contract => new()
    {
        Name = entity.Name,
        ResourceId = entity.Id,
        Type = entity.GetType().FullName ?? "Entity"
    };
}