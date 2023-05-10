using Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Common.Graph;
[Route("graph")]
public abstract class GraphControllerBase : ApiController
{
    protected readonly GraphService graphSvc;

    public GraphControllerBase(GraphService svc)
    {
        graphSvc = svc;
    }

    [HttpGet]
    public IActionResult Get() => ApiReturn(graphSvc.GraphId);
}