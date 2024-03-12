using System.ComponentModel.DataAnnotations;
using DomainModels = ExampleApp.Domain.Models;

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
    public static Role ToDto(this DomainModels.Role role)
        => new Role()
        {
            Id = role.Id,
            Name = role.Name
        };

    public static DomainModels.Role ToDomain(this Role role)
        => new DomainModels.Role(role.Id, role.Name);
}