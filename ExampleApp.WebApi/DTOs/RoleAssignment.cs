using System.ComponentModel.DataAnnotations;
using DomainModels = ExampleApp.Domain.Models;

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
    public static RoleAssignment ToDto(this DomainModels.RoleAssignment roleAssignment)
        => new RoleAssignment()
        {
            ExpiresOn = roleAssignment.ExpiresOn,
            GivenOn = roleAssignment.GivenOn,
            Role = roleAssignment.Role.ToDto()
        };

    public static DomainModels.RoleAssignment ToDbModel(this RoleAssignment roleAssignment, int personId)
        => new DomainModels.RoleAssignment
        (
            roleAssignment.GivenOn,
            roleAssignment.ExpiresOn,
            roleAssignment.Role.ToDomain()
        );
}