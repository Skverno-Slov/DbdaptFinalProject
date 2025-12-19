using System;
using System.Collections.Generic;

namespace StoreLib.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public byte UnitId { get; set; }

    public decimal Price { get; set; }

    public int SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public byte CategoryId { get; set; }

    public byte Discount { get; set; }

    public int StoredQuantity { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
