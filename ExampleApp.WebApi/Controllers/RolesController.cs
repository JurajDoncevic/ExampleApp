using Microsoft.AspNetCore.Mvc;
using ExampleApp.WebApi.DTOs;
using ExampleApp.Repositories;
using ExampleApp.Logging;

namespace ExampleApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ExampleApp.Logging.ILoggerAdapter<RolesController>? _logger;

    public RolesController(IRoleRepository roleRepository, ExampleApp.Logging.ILogger? logger = null)
    {
        _logger = logger?.ResolveLogger<RolesController>();
        _roleRepository = roleRepository;
    }

    // GET: api/Roles?id=1&id=2&id=3
    [HttpGet]
    public ActionResult<IEnumerable<Role>> GetRoles([FromQuery(Name = "id")] int[] ids)
    {

        var roles =
            ids.Length == 0
                ? _roleRepository.GetAll()
                : _roleRepository.GetByIds(ids); 

        if (roles.IsFailure)
        {
            _logger?.Error("Error getting roles: {Message}", roles.Message);
            return Problem(statusCode: 500, detail: roles.Message);
        }
        if (roles.IsException)
        {
            _logger?.Error("Exception getting roles: {Message}", roles.Message);
            return Problem(statusCode: 500, detail: roles.Message);
        }
        _logger?.Info("Returning {Count} roles", roles.Data.Count());
        return Ok(roles.Data.Select(DtoMapping.ToDto));
    }

    // GET: api/Roles/5
    [HttpGet("{id}")]
    public ActionResult<Role> GetRole(int id)
    {
        var role = _roleRepository.Get(id);
        if (role.IsFailure)
        {
            _logger?.Warn("Role not found: {Id}", id);
            return NotFound();
        }
        if (role.IsException)
        {
            _logger?.Error("Exception getting role: {Message}", role.Message);
            return Problem(statusCode: 500, detail: role.Message);
        }
        _logger?.Info("Returning role {Id}", role.Data.Id);
        return Ok(role.Data.ToDto());
    }

    // PUT: api/Roles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditRole(int id, Role role)
    {
        if (id != role.Id)
        {
            _logger?.Warn("Role ID mismatch: {Id}", id);
            return BadRequest();
        }

        var updatedRole = _roleRepository.Update(DtoMapping.ToDomain(role));
        if (updatedRole.IsFailure)
        {
            _logger?.Error("Error updating role: {Message}", updatedRole.Message);
            return Problem(statusCode: 404, detail: updatedRole.Message);
        }
        if (updatedRole.IsException)
        {
            _logger?.Error("Exception updating role: {Message}", updatedRole.Message);
            return Problem(statusCode: 500, detail: updatedRole.Message);
        }
        _logger?.Info("Role {Id} updated", role.Id);
        return NoContent();
    }

    // POST: api/Roles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Role> CreateRole(Role role)
    {
        var newRole = _roleRepository.Insert(DtoMapping.ToDomain(role));
        if (newRole.IsFailure)
        {
            _logger?.Error("Error creating role: {Message}", newRole.Message);
            return Problem(statusCode: 500, detail: newRole.Message);
        }
        if (newRole.IsException)
        {
            _logger?.Error("Exception creating role: {Message}", newRole.Message);
            return Problem(statusCode: 500, detail: newRole.Message);
        }
        _logger?.Info("Role created", newRole);
        return CreatedAtAction("GetRole", null);
    }

    // DELETE: api/Roles/5
    [HttpDelete("{id}")]
    public IActionResult DeleteRole(int id)
    {
        var removed = _roleRepository.Remove(id);
        if (removed.IsFailure)
        {
            _logger?.Warn("Role not found: {Id}", id);
            return Problem(statusCode: 404, detail: removed.Message);
        }
        if (removed.IsException)
        {
            _logger?.Error("Exception removing role: {Message}", removed.Message);
            return Problem(statusCode: 500, detail: removed.Message);
        }
        _logger?.Info("Role {Id} removed", id);
        return Ok();
    }
}
