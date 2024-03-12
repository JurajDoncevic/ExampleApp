using ExampleApp.Core;
using ExampleApp.Domain.Models;

namespace ExampleApp.Repositories;

/// <summary>
/// Facade interface for a Person repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IPersonRepository
    : IRepository<int, Person>,
      IAggregateRepository<int, Person>
{
  public Result AssignPersonToRole(int personId, int roleId, DateTime? expiresOn);
  public Result DismissPersonFromRole(int personId, int roleId);
}
