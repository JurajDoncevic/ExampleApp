using System;
using System.Collections.Generic;

namespace ExampleApp.WebApi.Models;

public partial class PersonRole
{
    public int PersonId { get; set; }

    public int RoleId { get; set; }

    public DateTime GivenOn { get; set; }

    public DateTime? ExpiresOn { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
