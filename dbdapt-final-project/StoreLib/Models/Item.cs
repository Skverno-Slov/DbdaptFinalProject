using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public int OrderId { get; set; }

    public string ProductCode { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal OrderedPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductCodeNavigation { get; set; } = null!;
}
