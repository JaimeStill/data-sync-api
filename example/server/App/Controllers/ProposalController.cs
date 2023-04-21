using App.Data;
using App.Models;
using App.Services;
using Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Route("api/[controller]")]
public class ProposalController : EntityController<Proposal, AppDbContext>
{    
    public ProposalController(ProposalService svc) : base(svc) { }
}