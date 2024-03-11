using System;
using System.Collections.Generic;

namespace ExampleApp.DataAccess.Sqlite.Models;

public partial class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public virtual ICollection<PersonRole> PersonRoles { get; set; } = new List<PersonRole>();
}
