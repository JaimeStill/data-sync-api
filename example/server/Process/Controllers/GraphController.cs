using Common.Graph;
using Contracts.Process;
using Microsoft.AspNetCore.Mvc;
using Process.Models;
using Process.Services;

namespace Process.Controllers;
public class GraphController : GraphControllerBase
{
    readonly ProcessSyncService svc;

    public GraphController(GraphService graph, ProcessSyncService svc)
    : base(graph)
    {
        this.svc = svc;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll() =>
        Ok(await svc.GetAll());

    [HttpGet("[action]/{state}")]
    public async Task<IActionResult> GetAllByState([FromRoute]ProcessState state) =>
        Ok(await svc.GetAllByState(state));

    [HttpGet("[action]/{resourceId:int}/{type}")]
    public async Task<IActionResult> GetByResource(
        [FromRoute]int resourceId,
        [FromRoute]string type
    ) => Ok(await svc.GetByResource(resourceId, type));

    [HttpPost("[action]")]
    public async Task<IActionResult> Complete([FromBody]Package package) =>
        ApiReturn(await svc.Complete(package));

    [HttpPost("[action]")]
    public async Task<IActionResult> Receive([FromBody] Package package) =>
        ApiReturn(await svc.Receive(package));

    [HttpPost("[action]/{status}")]
    public async Task<IActionResult> Reject(
        [FromBody] Package package,
        [FromRoute] string status
    ) => ApiReturn(await svc.Reject(package, status));

    [HttpPost("[action]/{status}")]
    public async Task<IActionResult> Return(
        [FromBody] Package package,
        [FromRoute] string status
    ) => ApiReturn(await svc.Return(package, status));

    [HttpPost("[action]/{status}")]
    public async Task<IActionResult> Sync(
        [FromBody] Package package,
        [FromRoute] string status
    ) => ApiReturn(await svc.Sync(package, status));

    [HttpDelete("[action]")]
    public async Task<IActionResult> Withdraw([FromBody] Package package) =>
        ApiReturn(await svc.Withdraw(package));
}