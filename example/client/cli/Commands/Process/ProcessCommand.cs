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
                new string[] { "--sync", "-y" },
                getDefaultValue: () => "http://localhost:5002/sync/process/",
                description: "Process Service SyncHub endpoint."
            ),
            new Option<string>(
                new string[] { "--process", "-pg", "-p" },
                getDefaultValue: () => "http://localhost:5002/graph/",
                description: "Process Service Graph endpoint."
            )
        },
        commands: new()
        {
            new ProcessCompleteCommand(),
            new ProcessGetCommand(),
            new ProcessListCommand(),
            new ProcessListenCommand(),
            new ProcessRejectCommand(),
            new ProcessReturnCommand(),
            new ProcessSyncCommand()
        }
    ) { }
}