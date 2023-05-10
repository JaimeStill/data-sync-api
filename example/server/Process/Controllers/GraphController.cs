using Common;
using Common.Graph;
using Contracts.Process;
using Microsoft.AspNetCore.Mvc;
using Process.Models;
using Process.Services;

namespace Process.Controllers;
public class GraphController : GraphControllerBase
{
    readonly PackageService svc;

    public GraphController(GraphService graph, PackageService svc)
    : base(graph)
    {
        this.svc = svc;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll() =>
        ApiReturn(await svc.GetAll());

    [HttpGet("[action]/{state}")]
    public async Task<IActionResult> GetAllByState([FromRoute]ProcessState state) =>
        ApiReturn(await svc.GetAllByState(state));

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetById(
        [FromRoute]int id
    ) => ApiReturn(await svc.GetById(id));

    [HttpGet("[action]/{resourceId:int}/{type}")]
    public async Task<IActionResult> GetByResource(
        [FromRoute]int resourceId,
        [FromRoute]string type
    ) => ApiReturn(await svc.GetByResource(resourceId, type));

    [HttpPost("[action]")]
    public async Task<IActionResult> Complete([FromBody]Package package) =>
        ApiReturn(await svc.Complete(package));

    [HttpPost("[action]")]
    public async Task<IActionResult> Receive([FromBody] Package package)
    {
        ApiResult<Package> result = await svc.Receive(package);
        return ApiReturn(result);
    }

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