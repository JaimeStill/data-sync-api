using System.CommandLine;
using Common.Cli;
using Contracts;
using Contracts.Sync;
using Sync;

namespace SyncCli.Commands;
public class ProposalListenerCommand : CliCommand
{
    public ProposalListenerCommand() : base(
        "proposal-listener",
        "Listen to a Proposal ApiSyncHub endpoint",
        new Func<string, Task>(Call),
        new()
        {
            new Option<string>(
                new string[] { "--endpoint", "e" },
                getDefaultValue: () => "http://localhost:5001/sync/proposal/",
                description: "Proposal Hub endpoint"
            )
        }
    ) { }

    static async Task Call(string endpoint)
    {
        await using ProposalSync client = new(endpoint);
        await client.Connect();        
        Console.WriteLine("Press Ctrl + C to exit...");

        while (true) { }
    }
}