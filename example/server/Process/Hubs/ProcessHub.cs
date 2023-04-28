using Process.Models;
using Sync.Hub;

namespace Process.Hubs;
public class ProcessHub : SyncHub<Package, IProcessHub> { }