using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class OrderInfo
{
    public int OrderInfoId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int UserId { get; set; }

    public short ReceiveCode { get; set; }

    public byte StatusId { get; set; }

    public virtual ICollection<OrderCompound> OrderCompounds { get; set; } = new List<OrderCompound>();

    public virtual Status Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
