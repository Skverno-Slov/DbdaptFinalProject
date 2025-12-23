namespace StoreLib.DTOs
{
    // dto класс для запросов в API (тот же product, но без навигационных свойств)
    public class ProductDto
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
    }
}
