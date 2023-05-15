# Data Sync API

> In progress

Integrate web socket broadcasts for API-based actions.

## CRUD synchronization

Demonstrates events being executed when CRUD-based actions are executed. In this example, events are broadcast on: `Add`, `Update`, and `Remove`.

https://github.com/JaimeStill/azure-demo/assets/14102723/833110d6-f51d-40c5-b94c-3b3aed226b49

The **left terminal** is the example [App API Server](./example/server/App/).

The **center terminal** is a [testing CLI](./example/client/cli/) for interfacing with the example APIs.

The **right terminal** is running the `datasync proposal listen` command from the testing CLI. This command uses a [`ProposalSync`](./example/Contracts/App/ProposalSync.cs) client to listen to the [`ProposalHub`](./example/server/App/Hubs/ProposalHub.cs) broadcasting events through the [`ProposalService`](./example/server/App/Services/ProposalService.cs).

## Customized Process-based Synchronization

Demonstrates cross-service events being executed when custom process-based actions are executed. In this example, events are broadcast on `Send/Receive`, `Sync`, and `Complete`.

https://github.com/JaimeStill/azure-demo/assets/14102723/76df3f26-5024-4f6d-a532-2bf4ee91fb48

The **top-left terminal** is the example [App API Server](./example/server/App/). It uses a [`PackageSync`](./example/Contracts/Process/PackageSync.cs) client, registered via [`SyncStartups`](./example/server/App/Sync/SyncStartups.cs) in [`Program.cs`](./example/server/App/Program.cs#L56), to listen to the [`PackageHub`](./example/server/Process/Hubs/PackageHub.cs) broadcasting events through the [`PackageService`](./example/server/Process/Services/PackageService.cs).

The **bottom-left** terminal is the example [Process Service API](./example/server/Process/).

The **center terminal** is a [testing CLI](./example/client/cli/) for interfacing with the example APIs.

The **right terminal** is running the `datasync package listen` command from the testing CLI. This command uses a [`PackageSync`](./example/Contracts/Process/PackageSync.cs) client to listen to the [`PackageHub`](./example/server/Process/Hubs/PackageHub.cs) broadcasting events through the [`PackageService`](./example/server/Process/Services/PackageService.cs).

### Graph API

Cross-service communication is established through the [Graph](./example/Common/Graph/) infrastructure:

> See [Graph API](https://github.com/JaimeStill/decentralized-staffing/blob/main/graph.md) for a detailed explanation of this infrastructure.

* [App - GraphController](./example/server/App/Controllers/GraphController.cs)
* [AppGraph](./example/Contracts/App/AppGraph.cs)
* [App - appsettings.json](./example/server/App/appsettings.json)
* [Process - GraphController](./example/server/Process/Controllers/GraphController.cs)
* [ProcessGraph](./example/Contracts/Process/ProcessGraph.cs)
* [Process - appsettings.json](./example/server/Process/appsettings.json)

## API Service Event Reactions

Demonstrates leveraging internal API services to react to sync events to synchronize dependent data from an external service. In this example, the `Remove` action triggers the Process Service to cleanup any incomplete `Package` items associated with the removed data.

https://github.com/JaimeStill/data-sync-api/assets/14102723/865e476b-6856-41e7-8500-cd131a3ff556

The **left terminal** is the example [App API Server](./example/server/App).

The **center terminal** is a [testing CLI](./example/client/cli/) for interfacing with the example APIs.

The **right terminal** is the example [Process Service API](./example/server/Process/). It uses its own [`ProposalSync`](./example/server/Process/Sync/ProposalSync.cs) client to add its own [`HandleOnRemove`](./example/server/Process/Sync/ProposalSync.cs#L32). It uses the new [`ExecuteServiceAction`](./src/dotnet/Sync/Client/SyncClient.cs#L79) method in the base `SyncClient` class to retrieve a required service and execute an action against that service. Because `SyncClient` instances are registered as **Singleton** instances, they must interface with **Scoped** services through a custom service scope rather than having these services injected directly.