using System.ComponentModel.DataAnnotations;

namespace StoreLib.DTOs
{
    public class ProductCardDto
    {
        public int ProductId { get; set; }
        [Display(Name = "Название")]
        public string ProductName { get; set; } = null!;
        [Display(Name = "Еденица измерения")]
        public string UnitName { get; set; } = null!;
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Display(Name = "Поставщик")]
        public string SupplierName { get; set; } = null!;
        [Display(Name = "Производитель")]
        public string ManufacturerName { get; set; } = null!;
        [Display(Name = "Категория")]
        public string CategoryName { get; set; } = null!;
        [Display(Name = "Скидка")]
        public byte Discount { get; set; }
        [Display(Name = "Количество на складе")]
        public int StoredQuantity { get; set; }
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public decimal DiscountedPrice
        {
            get
            {
                if (Discount > 0)
                    return Price * (100 - Discount) / 100;
                return Price;
            }
        }

        public bool IsDiscountHight
        {
            get => Discount > 15;
        }
    }
}
