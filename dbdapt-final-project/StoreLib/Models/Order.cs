using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int PersonId { get; set; }

    public short ReceiveCode { get; set; }

    public byte StatusId { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual Person Person { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
