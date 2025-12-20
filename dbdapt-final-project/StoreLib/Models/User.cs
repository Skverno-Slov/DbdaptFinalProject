using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Login { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public byte RoleId { get; set; }

    public int PersonId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Person Person { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
