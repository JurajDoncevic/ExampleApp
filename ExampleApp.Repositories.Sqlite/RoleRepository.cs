using ExampleApp.DataAccess.Sqlite.Data;
using DomainModels = ExampleApp.Domain.Models;
using ExampleApp.Core;
using ExampleApp.Domain.Models;


namespace ExampleApp.Repositories.Sqlite;

public class RoleRepository : IRoleRepository
{
    private readonly ExampleDbContext _context;

    public RoleRepository(ExampleDbContext context)
    {
        _context = context;
    }

    public bool Exists(Role model)
    {
        try
        {
            return _context.Roles.Contains(model.ToDbModel());
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
            return _context.Roles.Any(r => r.Id == id);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Role> Get(int id)
    {
        try
        {
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return Results.OnFailure<Role>("Role not found");
            }
            return Results.OnSuccess(role.ToDomain());
        }
        catch (Exception ex)
        {
            return Results.OnException<Role>(ex);
        }
    }

    public Result<IEnumerable<Role>> GetAll()
    {
        try
        {
            var roles = _context.Roles.ToList();
            return Results.OnSuccess(roles.Select(r => r.ToDomain()));
        }
        catch (Exception ex)
        {
            return Results.OnException<IEnumerable<Role>>(ex);
        }
    }

    public Result<IEnumerable<Role>> GetByIds(int[] ids)
    {
        try
        {
            var roles = _context.Roles
                .Where(r => ids.Contains(r.Id))
                .ToList();
            return Results.OnSuccess(roles.Select(r => r.ToDomain()));
        }
        catch (Exception ex)
        {
            return Results.OnException<IEnumerable<Role>>(ex);
        }
    }

    public Result Insert(Role model)
    {
        try
        {
            _context.Roles.Add(model.ToDbModel());
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
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return Results.OnFailure("Role not found");
            }
            _context.Roles.Remove(role);
            _context.SaveChanges();
            return Results.OnSuccess();
        }
        catch (Exception ex)
        {
            return Results.OnException(ex);
        }
    }

    public Result Update(Role model)
    {
        try
        {
            var role = _context.Roles.Find(model.Id);
            if (role == null)
            {
                return Results.OnFailure("Role not found");
            }
            role.Name = model.Name;
            _context.SaveChanges();
            return Results.OnSuccess();
        }
        catch (Exception ex)
        {
            return Results.OnException(ex);
        }
    }
}
