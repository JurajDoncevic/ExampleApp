using ExampleApp.Repositories;
using ExampleApp.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPersonRepository _personRepository;

    public PeopleController(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    // GET: api/People

    [HttpGet]
    public ActionResult<IEnumerable<Person>> GetAllPeople()
    {
        var people = _personRepository.GetAll();

        if (people.IsFailure)
        {
            return Problem(statusCode: 500, detail: people.Message);
        }
        if (people.IsException)
        {
            return Problem(statusCode: 500, detail: people.Message);
        }

        return Ok(people.Data.Select(DtoMapping.ToDto));
    }

    // GET: api/People/5
    [HttpGet("{id}")]
    public ActionResult<Person> GetPerson(int id)
    {
        var person = _personRepository.Get(id);
        if (person.IsFailure)
        {
            return NotFound();
        }
        if (person.IsException)
        {
            return Problem(statusCode: 500, detail: person.Message);
        }
        return Ok(person.Data.ToDto());
    }


    [HttpGet("Aggregate/{id}")]
    public ActionResult<PersonAggregate> GetPersonAggregate(int id)
    {
        var person = _personRepository.GetAggregate(id);
        if (person.IsFailure)
        {
            return NotFound();
        }
        if (person.IsException)
        {
            return Problem(statusCode: 500, detail: person.Message);
        }
        return Ok(person.Data);
    }

    [HttpPost("{personId}/AssignToRole/{roleId}")]
    public IActionResult AssignPersonToRole(int personId, int roleId, DateTime? expiresOn)
    {
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
        if (id != person.Id)
        {
            return BadRequest();
        }

        var updatedPerson = _personRepository.Update(person.ToDomain());
        if (updatedPerson.IsFailure)
        {
            return Problem(statusCode: 500, detail: updatedPerson.Message);
        }
        if (updatedPerson.IsException)
        {
            return Problem(statusCode: 500, detail: updatedPerson.Message);
        }

        return NoContent();
    }

    // POST: api/People
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Person> CreatePerson(Person person)
    {
        var newPerson = _personRepository.Insert(person.ToDomain());
        if (newPerson.IsFailure)
        {
            return Problem(statusCode: 500, detail: newPerson.Message);
        }
        if (newPerson.IsException)
        {
            return Problem(statusCode: 500, detail: newPerson.Message);
        }

        return CreatedAtAction("GetPerson", null);
    }

    // DELETE: api/People/5
    [HttpDelete("{id}")]
    public IActionResult DeletePerson(int id)
    {
        var removed = _personRepository.Remove(id);
        if (removed.IsFailure)
        {
            return Problem(statusCode: 404, detail: removed.Message);
        }
        if (removed.IsException)
        {
            return Problem(statusCode: 500, detail: removed.Message);
        }

        return Ok();
    }

}
