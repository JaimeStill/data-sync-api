using Contracts;
using Contracts.Graph;
using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers;

[Route("graph/[controller]")]
public class AppController : ControllerBase
{
    readonly AppGraph graph;

    public AppController(AppGraph graph)
    {
        this.graph = graph;
    }

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await graph.Initialize());

    [HttpGet("[action]")]
    public async Task<IActionResult> GetProposals() =>
        Ok(await graph.GetProposals());

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetProposal([FromRoute] int id) =>
        Ok(await graph.GetProposal(id));

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveProposal([FromBody] ProposalContract proposal) =>
        Ok(await graph.SaveProposal(proposal));

    [HttpDelete("[action]")]
    public async Task<IActionResult> RemoveProposal([FromBody] ProposalContract proposal) =>
        Ok(await graph.RemoveProposal(proposal));
}