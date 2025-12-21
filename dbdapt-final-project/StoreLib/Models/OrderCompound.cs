using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class OrderCompound
{
    public int OrderCompoundId { get; set; }

    public int OrderInfoId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal OrderedPrice { get; set; }

    public virtual OrderInfo OrderInfo { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
