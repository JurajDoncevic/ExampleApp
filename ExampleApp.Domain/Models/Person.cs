using ExampleApp.Core;

namespace ExampleApp.Domain.Models;
public class Person : AggregateRoot<int>
{
    private string _firstName;
    private string _lastName;
    private DateTime? _dateOfBirth;
    private readonly List<RoleAssignment> _roleAssignments;

    public string FirstName { get => _firstName; set => _firstName = value; }
    public string LastName { get => _lastName; set => _lastName = value; }
    public DateTime? DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
    public IReadOnlyList<RoleAssignment> RoleAssignments => _roleAssignments.ToList();

    public Person(int id, string firstName, string lastName, DateTime? dateOfBirth, IEnumerable<RoleAssignment>? roles = null) : base(id)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            throw new ArgumentException($"'{nameof(firstName)}' cannot be null or empty.", nameof(firstName));
        }

        if (string.IsNullOrEmpty(lastName))
        {
            throw new ArgumentException($"'{nameof(lastName)}' cannot be null or empty.", nameof(lastName));
        }

        _firstName = firstName;
        _lastName = lastName;
        _dateOfBirth = dateOfBirth;
        _roleAssignments = roles?.ToList() ?? new List<RoleAssignment>();
    }

    public bool AssignRole(Role role, DateTime? givenOn = null, DateTime? expiresOn = null)
    {
        givenOn ??= DateTime.Now;

        var roleAssignment = new RoleAssignment(givenOn.Value, expiresOn, role);

        _roleAssignments.Add(roleAssignment);

        return true;
    }

    public bool AssignRole(RoleAssignment roleAssignment)
    {
        return AssignRole(roleAssignment.Role, roleAssignment.GivenOn, roleAssignment.ExpiresOn);
    }

    public bool DismissFromRole(RoleAssignment roleAssignment)
    {
        return _roleAssignments.Remove(roleAssignment);
    }

    public bool DismissFromRole(Role role)
    {
        var targetAssignment = _roleAssignments.FirstOrDefault(ra => ra.Role.Equals(role));

        return targetAssignment != null &&
               _roleAssignments.Remove(targetAssignment);
    }

    public override bool Equals(object? obj)
    {
        return  obj is not null &&
                obj is Person person &&
               _id == person._id &&
               _firstName == person._firstName &&
               _lastName == person._lastName &&
               _dateOfBirth == person._dateOfBirth &&
               _roleAssignments.SequenceEqual(person._roleAssignments);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _firstName, _lastName, _dateOfBirth, _roleAssignments);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _firstName.Length <= 50, "First name lenght must be less than 50 characters"),
            (() => _lastName.Length <= 50, "Last name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_firstName.Trim()), "First name can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_lastName.Trim()), "Last name can't be null, empty, or whitespace")
            );
}
