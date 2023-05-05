using Common.Controllers;
using Contracts.Graph;
using Contracts.Process;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Route("graph/[controller]")]
public class ProcessController : ApiController
{
    readonly ProcessGraph graph;

    public ProcessController(ProcessGraph graph)
    {
        this.graph = graph;
    }

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await graph.Initialize());

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll() =>
        Ok(await graph.GetAll());

    [HttpGet("[action]/{state}")]
    public async Task<IActionResult> GetAllByState([FromRoute]ProcessState state) =>
        Ok(await graph.GetAllByState(state));

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetById(
        [FromRoute] int id
    ) => Ok(await graph.GetById(id));

    [HttpGet("[action]/{resourceId:int}/{type}")]
    public async Task<IActionResult> GetByResource(
        [FromRoute]int resourceId,
        [FromRoute]string type
    ) => Ok(await graph.GetByResource(resourceId, type));

    [HttpPost("[action]")]
    public async Task<IActionResult> Send([FromBody] PackageContract package) =>
        ApiReturn(await graph.Send(package));

    [HttpPost("[action]")]
    public async Task<IActionResult> Complete([FromBody] PackageContract package) =>
        ApiReturn(await graph.Complete(package));

    [HttpPost("[action]/{status}")]
    public async Task<IActionResult> Reject(
        [FromBody] PackageContract package,
        [FromRoute] string status
    ) => ApiReturn(await graph.Reject(package, status));

    [HttpPost("[action]/{status}")]
    public async Task<IActionResult> Return(
        [FromBody] PackageContract package,
        [FromRoute] string status
    ) => ApiReturn(await graph.Return(package, status));

    [HttpPost("[action]/{status}")]
    public async Task<IActionResult> Sync(
        [FromBody] PackageContract package,
        [FromRoute] string status
    ) => ApiReturn(await graph.Sync(package, status));

    [HttpDelete("[action]")]
    public async Task<IActionResult> Withdraw([FromBody] PackageContract package) =>
        ApiReturn(await graph.Withdraw(package));
}