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
                new string[] { "--sync", "-y" },
                getDefaultValue: () => "http://localhost:5001/sync/proposal/",
                description: "App Proposal SyncHub endpoint"
            ),
            new Option<string>(
                new string[] { "--app", "--ag", "-a" },
                getDefaultValue: () => "http://localhost:5001/graph/",
                description: "App Graph endpoint"
            ),
            new Option<string>(
                new string[] { "--process", "--pg", "-p" },
                getDefaultValue: () => "http://localhost:5002/graph/",
                description: "Process Service Graph endpoint"
            )
        },
        commands: new()
        {
            new ProposalGetCommand(),
            new ProposalListCommand(),
            new ProposalListenCommand(),
            new ProposalRemoveCommand(),
            new ProposalSaveCommand()
        }
    ) { }
}