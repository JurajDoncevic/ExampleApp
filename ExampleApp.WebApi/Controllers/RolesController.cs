using Microsoft.AspNetCore.Mvc;
using ExampleApp.WebApi.DTOs;
using ExampleApp.DataAccess.Sqlite;

namespace ExampleApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleRepository _roleRepository;

    public RolesController(RoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    // GET: api/Roles?id=1&id=2&id=3
    [HttpGet]
    public ActionResult<IEnumerable<Role>> GetRoles([FromQuery(Name = "id")] int[] ids)
    {
        // 500 on error
        // 200 with empty array if no roles
        // 200 with array of roles if roles
        // map role to DTO
        try
        {
            var roles = _roleRepository
                .GetAll()
                .Where(r => ids.Contains(r.Id));
            return Ok(roles.Select(r => r.ToDto()));
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    // GET: api/Roles/5
    [HttpGet("{id}")]
    public ActionResult<Role> GetRole(int id)
    {
        // 500 on error
        // 404 if role not found
        // 200 with role if role
        try
        {
            var role = _roleRepository.Get(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role.ToDto());
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    // PUT: api/Roles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditRole(int id, Role role)
    {
        try
        {
            if (id != role.Id)
            {
                return BadRequest();
            }
            if (_roleRepository.Update(id, role.ToDbModel()) == null)
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

    // POST: api/Roles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Role> CreateRole(Role role)
    {
        try
        {
            var createdRole = _roleRepository.Insert(role.ToDbModel());
            if (createdRole == null)
            {
                return Problem(statusCode: 500, detail: "Failed to create role");
            }
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole.ToDto());
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }

    // DELETE: api/Roles/5
    [HttpDelete("{id}")]
    public IActionResult DeleteRole(int id)
    {
        try
        {
            if (!_roleRepository.Delete(id))
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
}
