using System.ComponentModel.DataAnnotations;
using DbModels = ExampleApp.WebApi.Models;

namespace ExampleApp.WebApi.DTOs;

public class Role
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Role name can't be empty", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Role name can't be longer than 50 characters")]
    public string Name { get; set; } = string.Empty;
}

public static partial class DtoMapping
{
    public static Role ToDto(this DbModels.Role role)
        => new Role()
        {
            Id = role.Id,
            Name = role.Name
        };

    public static DbModels.Role ToDbModel(this Role role)
        => new DbModels.Role
        {
            Id = role.Id,
            Name = role.Name
        };
}