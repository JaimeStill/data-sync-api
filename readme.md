# Data Sync API

> In progress

Integrate web socket broadcasts for API-based actions.

**CRUD synchronization**  

Demonstrates events being executed when CRUD-based actions are executed. In this example, events are broadcast on: `Add`, `Update`, and `Remove`.

The left terminal is the example [App API Server](./example/server/App/).

The center terminal is a [testing CLI](./example/client/cli/) for interfacing with the example APIs.

The right terminal is running the `datasync proposal listen` command from the testing CLI. This command uses a [`ProposalSync`](./example/Contracts/Proposal/ProposalSync.cs) client to listen to the [`ProposalHub`](./example/server/App/Hubs/ProposalHub.cs) broadcasting events through the [`ProposalService`](./example/server/App/Services/ProposalService.cs).

https://github.com/JaimeStill/azure-demo/assets/14102723/833110d6-f51d-40c5-b94c-3b3aed226b49

**Customized Process-based Synchronization**

Demonstrates cross-service events being executed when custom process-based actions are executed. In this example, events are broadcast on `Send/Receive`, `Sync`, and `Complete`.

The top-left terminal is the example [App API Server](./example/server/App/). It uses a [`PackageSync`](./example/Contracts/Process/PackageSync.cs) client, registered via [`SyncStartups`](./example/server/App/Sync/SyncStartups.cs) in [`Program.cs`](./example/server/App/Program.cs#L56), to listen to the [`PackageHub`](./example/server/Process/Hubs/PackageHub.cs) broadcasting events through the [`PackageService`](./example/server/Process/Services/PackageService.cs).

The bottom-left terminal is the example [Process Service API](./example/server/Process/).

The cetner terminal is a [testing CLI](./example/client/cli/) for interfacing with the example APIs.

The right terminal is running the `datasync package listen` command from the testing CLI. This command uses a [`PackageSync`](./example/Contracts/Process/PackageSync.cs) client to listen to the [`PackageHub`](./example/server/Process/Hubs/PackageHub.cs) broadcasting events through the [`PackageService`](./example/server/Process/Services/PackageService.cs).

https://github.com/JaimeStill/azure-demo/assets/14102723/76df3f26-5024-4f6d-a532-2bf4ee91fb48