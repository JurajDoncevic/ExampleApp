namespace ExampleApp.WebApi.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PersonRole> PersonRoles { get; set; } = new List<PersonRole>();
}
