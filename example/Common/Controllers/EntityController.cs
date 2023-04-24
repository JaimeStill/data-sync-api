using Common.Schema;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Common.Controllers;
public abstract class EntityController<T, Db> : ControllerBase
    where T : Entity
    where Db : DbContext
{
    protected readonly IService<T, Db> baseSvc;

    public EntityController(IService<T, Db> svc)
    {
        baseSvc = svc;
    }

    [HttpGet("[action]")]
    public virtual async Task<IActionResult> Get() =>
        Ok(await baseSvc.Get());

    [HttpGet("[action]/{id:int}")]
    public virtual async Task<IActionResult> GetById([FromRoute]int id) =>
        Ok(await baseSvc.GetById(id));

    [HttpPost("[action]")]
    public virtual async Task<IActionResult> ValidateName([FromBody]T entity) =>
        Ok(await baseSvc.ValidateName(entity));
        
    [HttpPost("[action]")]
    public virtual async Task<IActionResult> Validate([FromBody]T entity) =>
        Ok(await baseSvc.Validate(entity));

    [HttpPost("[action]")]
    public virtual async Task<IActionResult> Save([FromBody]T entity) =>
        Ok(await baseSvc.Save(entity));

    [HttpDelete("[action]")]
    public virtual async Task<IActionResult> Remove([FromBody]T entity) =>
        Ok(await baseSvc.Remove(entity));
}