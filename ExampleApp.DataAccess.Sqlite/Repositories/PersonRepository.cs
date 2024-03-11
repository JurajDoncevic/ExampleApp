using ExampleApp.DataAccess.Sqlite.Data;
using ExampleApp.DataAccess.Sqlite.Models;
using ExampleApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.DataAccess.Sqlite;

public class PersonRepository : IRepository<int, Person>, IAggregateRepository<int, Person>
{
    private readonly ExampleDbContext _context;

    public PersonRepository(ExampleDbContext context)
    {
        _context = context;
    }

    public bool Delete(int id)
    {
        var person = _context.People.Find(id);
        if (person == null)
        {
            return false;
        }
        _context.People.Remove(person);
        return _context.SaveChanges() > 0;
    }

    public Person? Get(int id)
    {
        return _context.People
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Person> Get(params int[] ids)
    {
        return _context.People
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id))
            .ToList();
    }

    public Person? GetAggregate(int id)
    {
        return _context.People
            .Include(p => p.PersonRoles)
            .ThenInclude(pr => pr.Role)
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Person> GetAll()
    {
        return _context.People
            .AsNoTracking()
            .ToList();
    }

    public IEnumerable<Person> GetAllAggregates()
    {
        return _context.People
            .Include(p => p.PersonRoles)
            .ThenInclude(pr => pr.Role)
            .AsNoTracking()
            .ToList();
    }

    public Person Insert(Person entity)
    {
        _context.People.Add(entity);
        _context.SaveChanges();
        _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return entity;
    }

    public Person Update(int id, Person entity)
    {
        var person = _context.People.Find(id);
        if (person == null)
        {
            throw new InvalidOperationException("Person not found");
        }
        person.FirstName = entity.FirstName;
        person.LastName = entity.LastName;
        person.DateOfBirth = entity.DateOfBirth;
        _context.SaveChanges();
        _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return person;
    }

    public Person UpdateAggregate(int id, Person entity)
    {
        var person = _context.People
            .Include(p => p.PersonRoles)
            .FirstOrDefault(p => p.Id == id);
        if (person == null)
        {
            throw new InvalidOperationException("Person not found");
        }
        person.FirstName = entity.FirstName;
        person.LastName = entity.LastName;
        person.DateOfBirth = entity.DateOfBirth;
        person.PersonRoles = entity.PersonRoles;
        _context.SaveChanges();
        _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return person;
    }

    public bool AssignPersonToRole(int personId, int roleId, DateTime? expiresOn)
    {
        var person = _context.People.Find(personId);
        if (person == null)
        {
            return false;
        }

        var role = _context.Roles.Find(roleId);
        if (role == null)
        {
            return false;
        }

        var personRole = new PersonRole
        {
            Person = person,
            Role = role,
            GivenOn = DateTime.Now,
            ExpiresOn = expiresOn
        };

        _context.PersonRoles.Add(personRole);
        return _context.SaveChanges() > 0;
    }

    public bool DismissPersonFromRole(int personId, int roleId)
    {
        var personRole = _context.PersonRoles
            .FirstOrDefault(pr => pr.PersonId == personId && pr.RoleId == roleId);
        if (personRole == null)
        {
            return false;
        }

        _context.PersonRoles.Remove(personRole);
        return _context.SaveChanges() > 0;
    }
}
