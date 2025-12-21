using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class Status
{
    public byte StatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<OrderInfo> OrderInfos { get; set; } = new List<OrderInfo>();
}
