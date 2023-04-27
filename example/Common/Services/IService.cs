using Microsoft.EntityFrameworkCore;
using Common.Schema;

namespace Common.Services;

public interface IService<T, Db>
    where T : Entity
    where Db : DbContext
{
    Task<List<T>> Get();
    Task<T?> GetById(int id);
    Task<bool> ValidateName(T entity);
    Task<ValidationResult> Validate(T entity);
    Task<ApiResult<T>> Save(T entity);
    Task<ApiResult<int>> Remove(T entity);    
}