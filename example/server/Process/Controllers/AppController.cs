using Common.Controllers;
using Contracts.App;
using Microsoft.AspNetCore.Mvc;

namespace Process.Controllers;

[Route("graph/[controller]")]
public class AppController : ApiController
{
    readonly AppGraph graph;

    public AppController(AppGraph graph)
    {
        this.graph = graph;
    }

    [HttpGet]
    public async Task<IActionResult> Get() =>
        ApiReturn(await graph.Initialize());

    [HttpGet("[action]")]
    public async Task<IActionResult> GetProposals() =>
        ApiReturn(await graph.GetProposals());

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetProposal([FromRoute] int id) =>
        ApiReturn(await graph.GetProposal(id));

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveProposal([FromBody] ProposalContract proposal) =>
        ApiReturn(await graph.SaveProposal(proposal));

    [HttpDelete("[action]")]
    public async Task<IActionResult> RemoveProposal([FromBody] ProposalContract proposal) =>
        ApiReturn(await graph.RemoveProposal(proposal));
}