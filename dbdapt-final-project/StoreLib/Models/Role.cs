using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class Role
{
    public byte RoleId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
