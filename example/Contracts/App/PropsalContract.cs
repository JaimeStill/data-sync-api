using Common.Schema;

namespace Contracts.App;
public class ProposalContract : Contract
{
    public string Description { get; set; } = string.Empty;
    public override string Type { get; set; } = $"{ContractRoots.App}Proposal";

    public override string ToString() =>
        $"{Id} - {Name} - {Description}";
}