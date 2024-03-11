using ExampleApp.DataAccess.Sqlite;
using ExampleApp.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly PersonRepository _personRepository;

    public PeopleController(PersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    // GET: api/People

    [HttpGet]
    public ActionResult<IEnumerable<Person>> GetAllPeople()
    {
        try
        {
            var people = _personRepository.GetAll();
            return Ok(people.Select(p => p.ToDto()));
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
            var person = _personRepository.Get(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person.ToDto());
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }


    [HttpGet("Aggregate/{id}")]
    public ActionResult<PersonAggregate> GetPersonAggregate(int id)
    {
        try
        {
            var person = _personRepository.GetAggregate(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person.ToAggregateDto());
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    [HttpPost("{personId}/AssignToRole/{roleId}")]
    public IActionResult AssignPersonToRole(int personId, int roleId, DateTime? expiresOn)
    {
        // no change here since we didn't use a DTO to transfer the data
        try
        {
            if (_personRepository.AssignPersonToRole(personId, roleId, expiresOn))
            {
                return Ok();
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    [HttpPost("{personId}/DismissFromRole/{roleId}")]
    public IActionResult DismissPersonFromRole(int personId, int roleId)
    {
        // no change here since we didn't use a DTO to transfer the data
        try
        {
            if (_personRepository.DismissPersonFromRole(personId, roleId))
            {
                return Ok();
            }
            return NotFound();
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
            if (_personRepository.Update(id, person.ToDbModel()) == null)
            {
                return NotFound();
            }
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
            var createdPerson = _personRepository.Insert(person.ToDbModel());
            return CreatedAtAction(nameof(GetPerson), new { id = createdPerson.Id }, createdPerson.ToDto());
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
        // no change here since we didn't use a DTO to transfer the data
        try
        {
            if (_personRepository.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

}
