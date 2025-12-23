namespace StoreLib.Models;

public partial class Category
{
    public byte CategoryId { get; set; }

    public string Name { get; set; } = null!; // null! - свойство не может быть null

    public virtual ICollection<Product> Products { get; set; } = new List<Product>(); //Навигационное свойство для связи 1:М (у товара может быть 1 категория, категория может быть у нескльких товаров)
}
