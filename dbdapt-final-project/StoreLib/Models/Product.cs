using System.ComponentModel.DataAnnotations;

namespace StoreLib.Models;

public partial class Product
{
    public int ProductId { get; set; }
    [Display(Name = "Артикул")]
    public string ProductCode { get; set; } = null!;
    [Display(Name = "Название")]
    public string ProductName { get; set; } = null!;
    [Display(Name = "Еденица измерения")]
    public byte UnitId { get; set; }
    [Display(Name = "Цена")]
    public decimal Price { get; set; }
    [Display(Name = "Поставщик")]
    public int SupplierId { get; set; }
    [Display(Name = "Производитель")]
    public int ManufacturerId { get; set; }
    [Display(Name = "Категория")]
    public byte CategoryId { get; set; }
    [Display(Name = "Скидка")]
    public byte Discount { get; set; }
    [Display(Name = "Количество на складе")]
    public int StoredQuantity { get; set; }
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    public string? Photo { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
