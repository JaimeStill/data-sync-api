using Microsoft.AspNetCore.Mvc;

namespace Common.Controllers;
public abstract class ApiController : ControllerBase
{
    protected IActionResult ApiReturn<T>(ApiResult<T>? result) =>
        result is null || result.Error
            ? BadRequest(result)
            : Ok(result);    
}