using System.CommandLine;
using Common.Cli;

namespace SyncCli.Commands;
public class PackageCommand : CliCommand
{
    public PackageCommand() : base(
        "package",
        "Interface with Packages via the Process Service API",
        options: new()
        {
            new Option<string>(
                new string[] { "--sync", "-y" },
                getDefaultValue: () => "http://localhost:5002/sync/package",
                description: "Process Service Package SyncHub endpoint."
            ),
            new Option<string>(
                new string[] { "--process", "-pg", "-p" },
                getDefaultValue: () => "http://localhost:5002/graph/",
                description: "Process Service Graph endpoint."
            )
        },
        commands: new()
        {
            new PackageCompleteCommand(),
            new PackageGetCommand(),
            new PackageListCommand(),
            new PackageListenCommand(),
            new PackagePingCommand(),
            new PackageRejectCommand(),
            new PackageReturnCommand(),
            new PackageSyncCommand()
        }
    ) { }
}