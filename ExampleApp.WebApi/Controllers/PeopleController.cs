using ExampleApp.Repositories;
using ExampleApp.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using ExampleApp.Logging;
using ExampleApp.Core;

namespace ExampleApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPersonRepository _personRepository;
    private readonly ExampleApp.Logging.ILoggerAdapter<PeopleController>? _logger;

    public PeopleController(IPersonRepository personRepository, ExampleApp.Logging.ILogger? logger = null)
    {
        _logger = logger?.ResolveLogger<PeopleController>();
        _personRepository = personRepository;
    }

    // GET: api/People

    [HttpGet]
    public ActionResult<IEnumerable<Person>> GetAllPeople()
    {
        var people = _personRepository.GetAll();

        if (people.IsFailure)
        {
            _logger?.Error("Error getting people: {Message}", people.Message);
            return Problem(statusCode: 500, detail: people.Message);
        }
        if (people.IsException)
        {
            _logger?.Error("Exception getting people: {Message}", people.Message);
            return Problem(statusCode: 500, detail: people.Message);
        }
        var result = people.Data.Select(DtoMapping.ToDto);

        _logger?.Info("Returning {Count} people", result.Count());

        return Ok(result);
    }

    // GET: api/People/5
    [HttpGet("{id}")]
    public ActionResult<Person> GetPerson(int id)
    {
        var person = _personRepository.Get(id);
        if (person.IsFailure)
        {
            _logger?.Warn("Person not found: {Id}", id);
            return NotFound();
        }
        if (person.IsException)
        {
            _logger?.Error("Exception getting person: {Message}", person.Message);
            return Problem(statusCode: 500, detail: person.Message);
        }
        _logger?.Info("Returning person {Id}", person.Data.Id);

        return Ok(person.Data.ToDto());
    }


    [HttpGet("Aggregate/{id}")]
    public ActionResult<PersonAggregate> GetPersonAggregate(int id)
    {
        var person = _personRepository.GetAggregate(id);
        if (person.IsFailure)
        {
            _logger?.Warn("Person not found: {Id}", id);
            return NotFound();
        }
        if (person.IsException)
        {
            _logger?.Error("Exception getting person: {Message}", person.Message);
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
                _logger?.Info("Person {PersonId} assigned to role {RoleId}", personId, roleId);
                return Ok();
            }
            _logger?.Warn("Person {PersonId} not found", personId);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger?.Error("Exception assigning person {PersonId} to role {RoleId}: {Message}", personId, roleId, ex.Message);
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
                _logger?.Info("Person {PersonId} dismissed from role {RoleId}", personId, roleId);
                return Ok();
            }
            _logger?.Warn("Person {PersonId} or Role {RoleId} not found", personId, roleId);
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
            _logger?.Warn("Person ID mismatch: {Id} != {PersonId}", id, person.Id);
            return BadRequest();
        }

        var updatedPerson = _personRepository.Update(person.ToDomain());
        if (updatedPerson.IsFailure)
        {
            _logger?.Error("Error updating person: {Message}", updatedPerson.Message);
            return Problem(statusCode: 500, detail: updatedPerson.Message);
        }
        if (updatedPerson.IsException)
        {
            _logger?.Error("Exception updating person: {Message}", updatedPerson.Message);
            return Problem(statusCode: 500, detail: updatedPerson.Message);
        }
        _logger?.Info("Person {Id} updated", person.Id);
        return NoContent();
    }

    // POST: api/People
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Person> CreatePerson(Person person)
    {
        var insertion = _personRepository.Insert(person.ToDomain());
        if (insertion.IsFailure)
        {
            _logger?.Error("Error creating person: {Message}", insertion.Message);
            return Problem(statusCode: 500, detail: insertion.Message);
        }
        if (insertion.IsException)
        {
            _logger?.Error("Exception creating person: {Message}", insertion.Message);
            return Problem(statusCode: 500, detail: insertion.Message);
        }
        _logger?.Info("Person created", insertion);
        return CreatedAtAction("GetPerson", null);
    }

    // DELETE: api/People/5
    [HttpDelete("{id}")]
    public IActionResult DeletePerson(int id)
    {
        var removed = _personRepository.Remove(id);
        if (removed.IsFailure)
        {
            _logger?.Warn("Person not found: {Id}", id);
            return Problem(statusCode: 404, detail: removed.Message);
        }
        if (removed.IsException)
        {
            _logger?.Error("Exception removing person: {Message}", removed.Message);
            return Problem(statusCode: 500, detail: removed.Message);
        }
        _logger?.Info("Person {Id} removed", id);
        return Ok();
    }

}
