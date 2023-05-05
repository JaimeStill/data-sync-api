using Common.Schema;

namespace Contracts.Process;
public class PackageContract : Contract
{
    public override string Type => $"{ContractRoots.Process}Package";
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public ProcessState State { get; set; } = ProcessState.Pending;

    public ICollection<ResourceContract>? Resources { get; set; }

    public override string ToString() =>
        $"{Id} - {Name} - {State} - {Status} - {Description}";
}
