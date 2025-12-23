using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
