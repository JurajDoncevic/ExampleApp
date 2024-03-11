using System.ComponentModel.DataAnnotations;
using DbModels = ExampleApp.WebApi.Models;

namespace ExampleApp.WebApi.DTOs;

public class PersonAggregate
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name can't be empty", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name can't be empty", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public IEnumerable<RoleAssignment> RoleAssignments { get; set; } = Enumerable.Empty<RoleAssignment>();
}

public static partial class DtoMapping
{
    public static PersonAggregate ToAggregateDto(this DbModels.Person person)
        => new PersonAggregate()
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            RoleAssignments = person.PersonRoles == null
                            ? new List<RoleAssignment>()
                            : person.PersonRoles.Select(ra => ra.ToDto()).ToList()
        };

    public static DbModels.Person ToDbModel(PersonAggregate person)
        => new DbModels.Person {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            PersonRoles = person.RoleAssignments.Select(ra => ra.ToDbModel(person.Id)).ToList()
        };
}