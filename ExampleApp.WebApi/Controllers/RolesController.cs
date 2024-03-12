using Microsoft.AspNetCore.Mvc;
using ExampleApp.WebApi.DTOs;
using ExampleApp.Repositories;

namespace ExampleApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RolesController(IRoleRepository roleRepository)
    {
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
            return Problem(statusCode: 500, detail: roles.Message);
        }
        if (roles.IsException)
        {
            return Problem(statusCode: 500, detail: roles.Message);
        }

        return Ok(roles.Data.Select(DtoMapping.ToDto));
    }

    // GET: api/Roles/5
    [HttpGet("{id}")]
    public ActionResult<Role> GetRole(int id)
    {
        var role = _roleRepository.Get(id);
        if (role.IsFailure)
        {
            return NotFound();
        }
        if (role.IsException)
        {
            return Problem(statusCode: 500, detail: role.Message);
        }
        return Ok(role.Data.ToDto());
    }

    // PUT: api/Roles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditRole(int id, Role role)
    {
        if (id != role.Id)
        {
            return BadRequest();
        }

        var updatedRole = _roleRepository.Update(DtoMapping.ToDomain(role));
        if (updatedRole.IsFailure)
        {
            return Problem(statusCode: 404, detail: updatedRole.Message);
        }
        if (updatedRole.IsException)
        {
            return Problem(statusCode: 500, detail: updatedRole.Message);
        }

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
            return Problem(statusCode: 500, detail: newRole.Message);
        }
        if (newRole.IsException)
        {
            return Problem(statusCode: 500, detail: newRole.Message);
        }

        return CreatedAtAction("GetRole", null);
    }

    // DELETE: api/Roles/5
    [HttpDelete("{id}")]
    public IActionResult DeleteRole(int id)
    {
        var removed = _roleRepository.Remove(id);
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
