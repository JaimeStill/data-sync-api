using App.Models;
using App.Services;
using Common.Graph;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class GraphController : GraphControllerBase
{
    readonly ProposalService proposalSvc;

    public GraphController(GraphService graph, ProposalService proposalSvc)
    : base(graph)
    {
        this.proposalSvc = proposalSvc;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetProposals() =>
        Ok(await proposalSvc.Get());

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetProposal([FromRoute] int id) =>
        Ok(await proposalSvc.GetById(id));

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveProposal([FromBody] Proposal proposal) =>
        Ok((await proposalSvc.Save(proposal)).Data);

    [HttpDelete("[action]")]
    public async Task<IActionResult> RemoveProposal([FromBody] Proposal proposal) =>
        Ok((await proposalSvc.Remove(proposal)).Data);
}