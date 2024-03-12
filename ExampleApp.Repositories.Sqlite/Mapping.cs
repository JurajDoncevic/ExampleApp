using ExampleApp.Domain.Models;
using DbModels = ExampleApp.DataAccess.Sqlite.Models;
namespace ExampleApp.Repositories.Sqlite;
public static class Mapping
{
    public static Role ToDomain(this DbModels.Role role)
        => new Role(
            role.Id,
            role.Name
            );

    public static DbModels.Role ToDbModel(this Role role)
        => new DbModels.Role()
        {
            Id = role.Id,
            Name = role.Name
        };

    public static RoleAssignment ToDomain(this DbModels.PersonRole personRole)
        => new RoleAssignment(
            personRole.GivenOn,
            personRole.ExpiresOn,
            personRole.Role.ToDomain()
            );

    public static DbModels.PersonRole ToDbModel(this RoleAssignment roleAssignment, int personId)
        => new DbModels.PersonRole()
        {
            ExpiresOn = roleAssignment.ExpiresOn,
            GivenOn = roleAssignment.GivenOn,
            PersonId = personId,
            RoleId = roleAssignment.Role.Id
        };

    public static Person ToDomain(this DbModels.Person person)
        => new Person(
            person.Id,
            person.FirstName,
            person.LastName,
            person.DateOfBirth,
            person.PersonRoles.Select(ToDomain)
            );

    public static DbModels.Person ToDbModel(this Person person)
        => new DbModels.Person()
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            PersonRoles = person.RoleAssignments.Select(pr => pr.ToDbModel(person.Id)).ToList()
        };
}
