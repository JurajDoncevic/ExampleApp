using System.ComponentModel.DataAnnotations;
using DomainModels = ExampleApp.Domain.Models;

namespace ExampleApp.WebApi.DTOs;

public class Person
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name can't be null")]
    [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name can't be null")]
    [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime? DateOfBirth { get; set; }
}


public static partial class DtoMapping
{
    public static Person ToDto(this DomainModels.Person person)
        => new Person()
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth
        };

    public static DomainModels.Person ToDomain(this Person person)
        => new DomainModels.Person (
            person.Id,
            person.FirstName,
            person.LastName,
            person.DateOfBirth
        );
}