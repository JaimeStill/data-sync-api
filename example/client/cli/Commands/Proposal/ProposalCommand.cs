using System.CommandLine;
using Common.Cli;

namespace SyncCli.Commands;
public class ProposalCommand : CliCommand
{
    public ProposalCommand() : base(
        "proposal",
        "Interface with the App Proposal API",
        options: new()
        {
            new Option<string>(
                new string[] { "--sync", "-s" },
                getDefaultValue: () => "http://localhost:5001/sync/proposal/",
                description: "App Proposal SyncHub endpoint"
            ),
            new Option<string>(
                new string[] { "--graph", "-g" },
                getDefaultValue: () => "http://localhost:5001/graph/",
                description: "App Graph endpoint"
            )
        },
        commands: new()
        {
            new ProposalListCommand(),
            new ProposalListenCommand(),
            new ProposalRemoveCommand(),
            new ProposalSaveCommand()
        }
    ) { }
}