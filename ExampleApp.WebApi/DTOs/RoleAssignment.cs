using System.ComponentModel.DataAnnotations;
using DbModels = ExampleApp.WebApi.Models;

namespace ExampleApp.WebApi.DTOs;

public class RoleAssignment
{
    [Required(ErrorMessage = "Role to assign must be provided")]
    public Role Role { get; set; }
    [Required(ErrorMessage = "Date when the role was given on must be provided")]
    public DateTime GivenOn { get; set; }
    public DateTime? ExpiresOn { get; set; }
}


public static partial class DtoMapping
{
    public static RoleAssignment ToDto(this DbModels.PersonRole personRole)
        => new RoleAssignment()
        {
            ExpiresOn = personRole.ExpiresOn,
            GivenOn = personRole.GivenOn,
            Role = personRole.Role.ToDto()
        };

    public static DbModels.PersonRole ToDbModel(this RoleAssignment roleAssignment, int personId)
        => new DbModels.PersonRole {
            PersonId = personId,
            Role = roleAssignment.Role.ToDbModel(),
            GivenOn = roleAssignment.GivenOn,
            ExpiresOn = roleAssignment.ExpiresOn
        };
}