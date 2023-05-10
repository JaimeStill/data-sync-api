https://user-images.githubusercontent.com/14102723/235163667-4026f207-cd7e-467c-ab8b-19f64421ae8c.mp4

# `datasync -h`

```
Description:
  Data Sync CLI

Usage:
  datasync [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  package   Interface with Packages via the Process Service API
  proposal  Interface with the App Proposal API
```

## `datasync package -h`

> Running [listen.ps1](./listen.ps1) with `-Command package` will run `datasync package listen` from the compiled executable so you can debug the other proposal commands while still listening for sync events from the CLI. If you run `dotnet run -- package listen` then try to run any other commands, the compilation targets (`bin` and `obj`) will be locked by the listen process and the subsequent commands will not be able to execute.

```
Description:
  Interface with Packages via the Process Service API

Usage:
  datasync package [command] [options]

Options:
  -y, --sync <sync>             Process Service Package SyncHub endpoint. [default: http://localhost:5002/sync/package]
  -p, -pg, --process <process>  Process Service Graph endpoint. [default: http://localhost:5002/graph/]
  -?, -h, --help                Show help and usage information

Commands:
  complete  Complete a Package via the Process Service Graph API
  get       Get and list a Package record by ResourceId and Type via the Process Service Graph API
  list      Get and list Process Package records via the Process Graph API
  listen    Listen to a Package SyncHub endpoint
  ping      Ping the Package SyncHub endpoint
  reject    Reject a Package via the Process Service Graph API
  return    Return a Package via the Process Service Graph API
  sync      Sync a Package via the Process Service Graph API
```

### `datasync package complete -h`

```
Description:
  Complete a Package via the Process Service Graph API

Usage:
  datasync package complete [options]

Options:
  -i, --id <id>   Package ID
  -?, -h, --help  Show help and usage information
```

### `datasync package get -h`

```
Description:
  Get and list a Package record by ResourceId and Type via the Process Service Graph API

Usage:
  datasync package get [options]

Options:
  -i, --id <id>      Package ID (if type is not provided). ID for the resource referenced by a Resource (if type is provided).
  -t, --type <type>  Type of the resource referenced by a Resource.
  -?, -h, --help     Show help and usage information
```

### `datasync package list -h`

> `--state` is optional and will retrieve all packages if not provided.

```
Description:
  Get and list Process Package records via the Process Graph API

Usage:
  datasync package list [options]

Options:
  -s, --state <Complete|Pending|Rejected|Returned|Withdrawn>  Package State []
  -?, -h, --help                                              Show help and usage information
```

### `datasync package listen -h`

```
Description:
  Get and list Process Package records via the Process Graph API

Usage:
  datasync package list [options]

Options:
  -s, --state <Complete|Pending|Rejected|Returned|Withdrawn>  Package State []
  -?, -h, --help                                              Show help and usage information


PS G:\code\proto\data-sync-api\example\client\cli> dotnet run -- package listen -h\
Description:
  Listen to a Package SyncHub endpoint

Usage:
  datasync package listen [options]

Options:
  -?, -h, --help  Show help and usage information
```

### `datasync package ping -h`

```
Description:
  Ping the Package SyncHub endpoint

Usage:
  datasync package ping [options]

Options:
  -?, -h, --help  Show help and usage information
```

### `datasync package reject -h`

```
Description:
  Reject a Package via the Process Service Graph API

Usage:
  datasync package reject [options]

Options:
  -i, --id <id>          Package ID
  -s, --status <status>  Package Status
  -?, -h, --help         Show help and usage information
```

### `datasync package return -h`

```
Description:
  Return a Package via the Process Service Graph API

Usage:
  datasync package return [options]

Options:
  -i, --id <id>          Package ID
  -s, --status <status>  Package Status
  -?, -h, --help         Show help and usage information
```

### `datasync package sync -h`

```
Description:
  Sync a Package via the Process Service Graph API

Usage:
  datasync package sync [options]

Options:
  -i, --id <id>          Package ID
  -s, --status <status>  Package Status
  -?, -h, --help         Show help and usage information
```

## `datasync proposal -h`

> Running [listen.ps1](./listen.ps1) will run `datasync proposal listen` from the compiled executable so you can debug the other proposal commands while still listening for sync events from the CLI. If you run `dotnet run -- proposal listen` then try to run any other commands, the compilation targets (`bin` and `obj`) will be locked by the listen process and the subsequent commands will not be able to execute.

```
Description:
  Interface with the App Proposal API

Usage:
  datasync proposal [command] [options]

Options:
  -y, --sync <sync>              App Proposal SyncHub endpoint [default: http://localhost:5001/sync/proposal]
  -a, --ag, --app <app>          App Graph endpoint [default: http://localhost:5001/graph/]
  -p, --pg, --process <process>  Process Service Graph endpoint [default: http://localhost:5002/graph/]
  -?, -h, --help                 Show help and usage information

Commands:
  get       Get a Proposal via the App Graph API
  list      Get and list Proposal records via the App Graph API
  listen    Listen to a Proposal ApiSyncHub endpoint
  ping      Ping the Proposal ApiSyncHub endpoint
  remove    Remove a Proposal via the App Graph API
  save      Save a Proposal via the App Graph API
  send      Send a Proposal as a Package Resource to the Process Service via the Process Graph API
  withdraw  Withdraw a Proposal Package via the Process Service Graph API
```

### `datasync proposal get -h`

```
Description:
  Get a Proposal via the App Graph API

Usage:
  datasync proposal get [options]

Options:
  -i, --id <id>   Proposal ID
  -?, -h, --help  Show help and usage information
```

### `datasync proposal list -h`

```
Description:
  Get and list Proposal records via the App Graph API

Usage:
  datasync proposal list [options]

Options:
  -?, -h, --help  Show help and usage information
```

### `datasync proposal listen -h`

```
Description:
  Listen to a Proposal ApiSyncHub endpoint

Usage:
  datasync proposal listen [options]

Options:
  -?, -h, --help  Show help and usage information
```

### `datasync proposal ping -h`

```
Description:
  Ping the Proposal ApiSyncHub endpoint

Usage:
  datasync proposal ping [options]

Options:
  -?, -h, --help  Show help and usage information
```

### `datasync proposal remove -h`

```
Description:
  Remove a Proposal via the App Graph API

Usage:
  datasync proposal remove [options]

Options:
  -i, --id <id>   Proposal ID
  -?, -h, --help  Show help and usage information
```

### `datasync proposal save -h`

> If no `--id` is provided, a new `Proposal` is created

```
Description:
  Save a Proposal via the App Graph API

Usage:
  datasync proposal save [options]

Options:
  -n, --name <name>                        Proposal name [default: CLI Generated Proposal]
  -d, --desc, --description <description>  Proposal description [default: A Proposal generated by the Sync CLI]
  -i, --id <id>                            Proposal ID [default: 0]
  -?, -h, --help                           Show help and usage information
```

### `datasync proposal send -h`

```
Description:
  Send a Proposal as a Package Resource to the Process Service via the Process Graph API

Usage:
  datasync proposal send [options]

Options:
  -n, --name <name>                        Package name [default: CLI Generated Package]
  -d, --desc, --description <description>  Package description [default: A Package generated by the Sync CLI]
  -i, --id <id>                            Proposal ID
  -?, -h, --help                           Show help and usage information
```

### `datasync proposal withdraw -h`

```
Description:
  Withdraw a Proposal Package via the Process Service Graph API

Usage:
  datasync proposal withdraw [options]

Options:
  -i, --id <id>   Proposal ID
  -?, -h, --help  Show help and usage information
```