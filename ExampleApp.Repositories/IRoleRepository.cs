using ExampleApp.Core;
using ExampleApp.Domain.Models;

namespace ExampleApp.Repositories;

/// <summary>
/// Facade interface for a Role repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IRoleRepository 
    : IRepository<int, Role>
{
    /// <summary>
    /// Get roles with the given IDs
    /// </summary>
    /// <param name="ids">Selection IDs</param>
    /// <returns>IEnumerable of Roles with given IDs</returns>
    public Result<IEnumerable<Role>> GetByIds(int[] ids);
}
