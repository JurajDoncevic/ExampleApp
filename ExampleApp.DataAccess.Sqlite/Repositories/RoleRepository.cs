using ExampleApp.DataAccess.Sqlite.Data;
using ExampleApp.DataAccess.Sqlite.Models;
using ExampleApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.DataAccess.Sqlite;

public class RoleRepository : IRepository<int, Role>
{
    private readonly ExampleDbContext _context;

    public RoleRepository(ExampleDbContext context)
    {
        _context = context;
    }

    public bool Delete(int id)
    {
        var role = _context.Roles.Find(id);
        if (role == null)
        {
            return false;
        }
        _context.Roles.Remove(role);
        return _context.SaveChanges() > 0;
    }

    public Role? Get(int id)
    {
        return _context.Roles
            .AsNoTracking()
            .FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Role> Get(params int[] ids)
    {
        return _context.Roles
            .AsNoTracking()
            .Where(r => ids.Contains(r.Id))
            .ToList();
    }

    public IEnumerable<Role> GetAll()
    {
        return _context.Roles.ToList();
    }

    public Role? Insert(Role entity)
    {
        _context.Roles.Add(entity);
        _context.SaveChanges();
        _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return entity;
    }

    public Role? Update(int id, Role entity)
    {
        var role = _context.Roles.Find(id);
        if (role == null)
        {
            return null;
        }
        role.Name = entity.Name;
        _context.SaveChanges();
        _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return role;
    }
}
