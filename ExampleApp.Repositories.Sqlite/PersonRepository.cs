using ExampleApp.DataAccess.Sqlite.Data;
using ExampleApp.Core;
using ExampleApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Repositories.Sqlite;

public class PersonRepository : IPersonRepository
{
    private readonly ExampleDbContext _context;

    public PersonRepository(ExampleDbContext context)
    {
        _context = context;
    }

    public Result AssignPersonToRole(int personId, int roleId, DateTime? expiresOn)
    {
        try
        {
            _context.PersonRoles.Add(new DataAccess.Sqlite.Models.PersonRole
            {
                PersonId = personId,
                RoleId = roleId,
                ExpiresOn = expiresOn
            });

            if (_context.SaveChanges() > 0)
            {
                return Results.OnSuccess();
            }

            return Results.OnFailure("Couldn't add person to role");
        }
        catch (Exception ex)
        {
            return Results.OnException(ex);
        }
    }

    public Result DismissPersonFromRole(int personId, int roleId)
    {
        try
        {
            var personRole = _context.PersonRoles
                .FirstOrDefault(pr => pr.PersonId == personId && pr.RoleId == roleId);
            if (personRole == null)
            {
                return Results.OnFailure("Person not found in role");
            }
            _context.PersonRoles.Remove(personRole);
            if (_context.SaveChanges() > 0)
            {
                return Results.OnSuccess();
            }
            return Results.OnFailure();
        }
        catch (Exception ex)
        {
            return Results.OnException(ex);
        }
    }

    public bool Exists(Person model)
    {
        try
        {
            return _context.People.Contains(model.ToDbModel());
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Exists(int id)
    {
        try
        {
            return _context.People.Any(p => p.Id == id);
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<Person> Get(int id)
    {
        try
        {
            var person = _context.People.Find(id);
            if (person == null)
            {
                return Results.OnFailure<Person>("Person not found");
            }
            return Results.OnSuccess(person.ToDomain());
        }
        catch (Exception ex)
        {
            return Results.OnException<Person>(ex);
        }
    }

    public Result<Person> GetAggregate(int id)
    {
        try
        {
            var person = _context.People
                .Include(p => p.PersonRoles)
                .ThenInclude(pr => pr.Role)
                .FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                return Results.OnFailure<Person>("Person not found");
            }
            return Results.OnSuccess(person.ToDomain());
        }
        catch (Exception ex)
        {
            return Results.OnException<Person>(ex);
        }
    }

    public Result<IEnumerable<Person>> GetAll()
    {
        try
        {
            var people = _context.People.ToList();
            return Results.OnSuccess(people.Select(p => p.ToDomain()));
        }
        catch (Exception ex)
        {
            return Results.OnException<IEnumerable<Person>>(ex);
        }
    }

    public Result<IEnumerable<Person>> GetAllAggregates()
    {
        try
        {
            var people = _context.People
                .Include(p => p.PersonRoles)
                .ThenInclude(pr => pr.Role)
                .ToList();
            return Results.OnSuccess(people.Select(p => p.ToDomain()));
        }
        catch (Exception ex)
        {
            return Results.OnException<IEnumerable<Person>>(ex);
        }
    }

    public Result Insert(Person model)
    {
        try
        {
            _context.People.Add(model.ToDbModel());
            _context.SaveChanges();
            return Results.OnSuccess();
        }
        catch (Exception ex)
        {
            return Results.OnException(ex);
        }
    }

    public Result Remove(int id)
    {
        try
        {
            var person = _context.People.Find(id);
            if (person == null)
            {
                return Results.OnFailure("Person not found");
            }
            _context.People.Remove(person);
            _context.SaveChanges();
            return Results.OnSuccess();
        }
        catch (Exception ex)
        {
            return Results.OnException(ex);
        }
    }

    public Result Update(Person model)
    {
        try
        {
            var person = _context.People.Find(model.Id);
            if (person == null)
            {
                return Results.OnFailure("Person not found");
            }
            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.DateOfBirth = model.DateOfBirth;
            _context.SaveChanges();
            return Results.OnSuccess();
        }
        catch (Exception ex)
        {
            return Results.OnException(ex);
        }
    }

    public Result UpdateAggregate(Person model)
    {
        try
        {
            var person = _context.People
                .Include(p => p.PersonRoles)
                .FirstOrDefault(p => p.Id == model.Id);
            if (person == null)
            {
                return Results.OnFailure("Person not found");
            }
            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.DateOfBirth = model.DateOfBirth;
            person.PersonRoles = model.RoleAssignments.Select(pr => pr.ToDbModel(person.Id)).ToList();
            _context.SaveChanges();
            return Results.OnSuccess();
        }
        catch (Exception ex)
        {
            return Results.OnException(ex);
        }
    }
}
