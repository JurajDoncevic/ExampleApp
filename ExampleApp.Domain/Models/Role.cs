using ExampleApp.Core;

namespace ExampleApp.Domain.Models;
public class Role : Entity<int>
{
    private string _name;

    public Role(int id, string name) : base(id)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        _name = name;
    }

    public string Name { get => _name; set => _name = value; }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
               obj is Role role &&
               Id.Equals(role.Id) &&
               Name.Equals(role.Name);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _name.Length <= 50, "Role name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_name.Trim()), "Role name can't be null, empty, or whitespace")
            );
}

