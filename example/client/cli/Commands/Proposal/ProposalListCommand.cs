using Common.Cli;
using Contracts.App;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class ProposalListCommand : CliCommand
{
    public ProposalListCommand() : base(
        "list",
        "Get and list Proposal records via the App Graph API",
        new Func<string, Task>(Call)
    ) { }

    static async Task Call(string app)
    {
        AppGraph ag = App.GetAppGraph(app);

        Console.WriteLine("Retrieving Proposals");
        List<ProposalContract>? result = await ag.GetProposals();

        Console.WriteLine(
            result?.Count < 1
                ? "No Proposals Found"
                : "Proposals:"
        );

        result?.ForEach(App.PrintProposal);
    }
}