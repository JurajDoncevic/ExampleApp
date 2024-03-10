using ExampleApp.WebApi.Data;
using ExampleApp.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly ExampleDbContext _context;

    public PeopleController(ExampleDbContext context)
    {
        _context = context;
    }

    // GET: api/People
    [HttpGet]
    public ActionResult<IEnumerable<Person>> GetAllPeople()
    {
        try
        {
            var people = _context.People;
            if (people.Count() == 0)
            {
                return Ok(new Person[0]);
            }
            return Ok(people);
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    // GET: api/People/5
    [HttpGet("{id}")]
    public ActionResult<Person> GetPerson(int id)
    {
        try
        {
            var person = _context.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }


    [HttpGet("Aggregate/{id}")]
    public ActionResult<Person> GetPersonAggregate(int id)
    {
        try
        {
            // this is an aggregate, so it has have roles that are assigned to the person
            var person = _context.People
                .Include(p => p.PersonRoles)
                .ThenInclude(pr => pr.Role)
                .FirstOrDefault(p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    [HttpPost("{personId}/AssignToRole/{roleId}")]
    public IActionResult AssignPersonToRole(int personId, int roleId, DateTime? expiresOn)
    {
        try
        {
            var person = _context.People.Find(personId);
            if (person == null)
            {
                return NotFound();
            }

            var role = _context.Roles.Find(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var personRole = new PersonRole
            {
                Person = person,
                Role = role,
                GivenOn = DateTime.Now,
                ExpiresOn = expiresOn
            };

            _context.PersonRoles.Add(personRole);
            _context.SaveChanges();

            return Ok(personRole);
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    [HttpPost("{personId}/DismissFromRole/{roleId}")]
    public IActionResult DismissPersonFromRole(int personId, int roleId)
    {
        try
        {
            var personRole = _context.PersonRoles
                .FirstOrDefault(pr => pr.PersonId == personId && pr.RoleId == roleId);
            if (personRole == null)
            {
                return NotFound();
            }

            _context.PersonRoles.Remove(personRole);
            _context.SaveChanges();

            return Ok(personRole);
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }


    // PUT: api/People/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditPerson(int id, Person person)
    {
        try
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            _context.Entry(person).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    // POST: api/People
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Person> CreatePerson(Person person)
    {
        try
        {
            _context.People.Add(person);
            _context.SaveChanges();
            // person is now populated with the new id
            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    // DELETE: api/People/5
    [HttpDelete("{id}")]
    public IActionResult DeletePerson(int id)
    {
        try
        {
            var person = _context.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }
            _context.People.Remove(person);
            _context.SaveChanges();
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

}
