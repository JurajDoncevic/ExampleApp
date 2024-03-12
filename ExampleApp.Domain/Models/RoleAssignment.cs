using ExampleApp.Core;

namespace ExampleApp.Domain.Models;
public class RoleAssignment : ValueObject
{
    private DateTime _givenOn;
    private DateTime? _expiresOn;
    private Role _role;

    public RoleAssignment(DateTime givenOn, DateTime? expiresOn, Role role)
    {
        _givenOn = givenOn;
        _expiresOn = expiresOn;
        _role = role ?? throw new ArgumentNullException(nameof(role));
    }

    public DateTime GivenOn { get => _givenOn; set => _givenOn = value; }
    public DateTime? ExpiresOn { get => _expiresOn; set => _expiresOn = value; }
    public Role Role { get => _role; set => _role = value; }

    public override bool Equals(object? obj)
    {
        return  obj is not null &&
                obj is RoleAssignment assignment &&
               _givenOn == assignment._givenOn &&
               _expiresOn == assignment._expiresOn &&
               _role.Equals(assignment._role);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_givenOn, _expiresOn, _role);
    }

    public override Result IsValid()
        => Validation.Validate(
                (() => _role != null, "Role can't be null"),
                (() => _givenOn != null, "Date when the role was given on can't be null"),
                (() => _expiresOn != null ? _expiresOn >= _givenOn : true, "Role assignment can't expires sooner than it was given")
            );
}
