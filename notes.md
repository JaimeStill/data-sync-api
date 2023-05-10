# Notes

## Todo

* Build out infrastructure that supports instantiating shorter-lived services inside of a Singleton Sync client with custom-registered event behaviors (i.e. - initialize `AppDbContext` and update records associated with external data affected by a socket event).

* Build out a web client that holistically demonstrates the power of this infrastructure.

* Extract all unnecessary infrastructure from `src` and move it to `example/Common` (i.e. - all infrastructure associated with `ApiSync` and `ProcessSync`). While these are great examples, they aren't needed as part of the source packages.

* Build out holistic documentation for the finalized source and example infrastructure.

## Links

* [how to specify local modules as npm package dependencies](https://stackoverflow.com/a/38417065/3971984)
* [waiting for your ASP.NET Core app to be ready from an IHostedService in .NET 6](https://andrewlock.net/finding-the-urls-of-an-aspnetcore-app-from-a-hosted-service-in-dotnet-6/)