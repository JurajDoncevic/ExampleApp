using System.ComponentModel.DataAnnotations;
using DomainModels = ExampleApp.Domain.Models;

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
    public static PersonAggregate ToAggregateDto(this DomainModels.Person person)
        => new PersonAggregate()
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            RoleAssignments = person.RoleAssignments == null
                            ? new List<RoleAssignment>()
                            : person.RoleAssignments.Select(ra => ra.ToDto()).ToList()
        };

    public static DomainModels.Person ToDomain(PersonAggregate person)
        => new DomainModels.Person (
            person.Id,
            person.FirstName,
            person.LastName,
            person.DateOfBirth,
            person.RoleAssignments.Select(ra => ra.ToDbModel(person.Id))
        );
}