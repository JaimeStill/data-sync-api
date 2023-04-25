using Microsoft.AspNetCore.Mvc;

namespace Common.Graph;
[Route("graph")]
public abstract class GraphControllerBase : ControllerBase
{
    protected readonly GraphService graphSvc;

    public GraphControllerBase(GraphService svc)
    {
        graphSvc = svc;
    }

    [HttpGet]
    public IActionResult Get() => Ok(graphSvc.GraphId);
}