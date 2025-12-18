using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string Manufacturer1 { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
