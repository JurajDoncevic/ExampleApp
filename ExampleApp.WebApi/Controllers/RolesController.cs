using ExampleApp.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExampleApp.WebApi.DTOs;

namespace ExampleApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly ExampleDbContext _context;
    public RolesController(ExampleDbContext context)
    {
        _context = context;
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
            var roles = _context.Roles.Where(r => ids.Contains(r.Id)).ToList();
            if (roles.Count == 0)
            {
                return Ok(new List<Role>());
            }
            return Ok(roles.Select(r => r.ToDto()).ToList());
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
            var role = _context.Roles.Find(id);
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
            _context.Entry(role.ToDbModel()).State = EntityState.Modified;
            _context.SaveChanges();
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
            _context.Roles.Add(role.ToDbModel());
            _context.SaveChanges();
            // role is now populated with the new id
            return CreatedAtAction("GetRole", new { id = role.Id }, role);
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
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            _context.Roles.Remove(role);
            _context.SaveChanges();
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, detail: ex.Message);
        }
    }
}
