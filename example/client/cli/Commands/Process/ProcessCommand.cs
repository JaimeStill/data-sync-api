using System.CommandLine;
using Common.Cli;

namespace SyncCli.Commands;
public class ProcessCommand : CliCommand
{
    public ProcessCommand() : base(
        "process",
        "Interface with the Process Service API",
        options: new()
        {
            new Option<string>(
                new string[] { "--sync", "-s" },
                getDefaultValue: () => "http://localhost:5002/sync/process/",
                description: "Process Service SyncHub endpoint"
            ),
            new Option<string>(
                new string[] { "--graph", "-g" },
                getDefaultValue: () => "http://localhost:5002/graph/"
            )
        },
        commands: new()
        {
            new ProcessGetCommand(),
            new ProcessListCommand(),
            new ProcessListenCommand()
        }
    ) { }
}