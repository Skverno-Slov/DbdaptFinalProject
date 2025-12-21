using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;
using System.Threading.Tasks;

namespace StoreLib.Services
{
    public class ProductService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        public async Task<List<Product>> GetProductsAsync()
            => await _context.Products.ToListAsync();

        public async Task<Product>? GetProductAsync(int id)
            => await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        public async Task<Product>? GetProductByCodeAsync(string code)
            => await _context.Products.FirstOrDefaultAsync(p => p.ProductCode == code);

        public async Task ChangeProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public bool IsDiscountValid(byte discount)
        {
            if (discount >= 100)
                return false;
            return true;
        }

        public async Task<ProductCardDto> GetProductCard(int id)
            => await _context.Products
                .Select(p => new ProductCardDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitName = p.Unit.Name,
                    Price = p.Price,
                    SupplierName = p.Supplier.Name,
                    ManufacturerName = p.Manufacturer.Name,
                    CategoryName = p.Category.Name,
                    Discount = p.Discount,
                    StoredQuantity = p.StoredQuantity,
                    Description = p.Description,
                    Photo = p.Photo
                })
                .FirstOrDefaultAsync(p => p.ProductId == id)
                ?? throw new ArgumentNullException("Товар не найден");

        public IQueryable<ProductCardDto> GetProductCards()
        {
            return _context.Products
            .Include(p => p.Category)
            .Include(p => p.Manufacturer)
            .Include(p => p.Supplier)
            .Include(p => p.Unit)
            .Select(p => new ProductCardDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitName = p.Unit.Name,
                Price = p.Price,
                SupplierName = p.Supplier.Name,
                ManufacturerName = p.Manufacturer.Name,
                CategoryName = p.Category.Name,
                Discount = p.Discount,
                StoredQuantity = p.StoredQuantity,
                Description = p.Description,
                Photo = p.Photo
            });
        }

        public IQueryable<ProductCardDto> ApplyDescriptionFilter(string? description,
                                                                 IQueryable<ProductCardDto> products)
        {
            if (String.IsNullOrWhiteSpace(description))
                return products;

            var filters = description.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var filter in filters)
            {
                products = products
                    .Where(p => p.Description.ToLower()
                    .Contains(filter.ToLower()));
            }

            return products;
        }

        public IQueryable<ProductCardDto> ApplyManufacturerFilter(string manufacturerName,
                                                            IQueryable<ProductCardDto> products)
        {
            if (string.IsNullOrWhiteSpace(manufacturerName))
                return products;

            return products
                .Where(p => p.ManufacturerName == manufacturerName);
        }

        public IQueryable<ProductCardDto> ApplyMaxPriceFilter(decimal? maxPrice,
                                                              IQueryable<ProductCardDto> products)
        {
            if (!maxPrice.HasValue || maxPrice <= 0)
                return products;

            return products
            .Where(p => (p.Discount > 0)
                ? p.Price * (100 - p.Discount) / 100 <= maxPrice.Value
                : p.Price <= maxPrice);
        }

        public IQueryable<ProductCardDto> ApplyDiscountedFilter(bool isApply, IQueryable<ProductCardDto> products)
        {
            if (!isApply)
                return products;

            return products
                    .Where(p => p.Discount > 0);
        }

        public IQueryable<ProductCardDto> ApplyInStockFilter(bool isApply, IQueryable<ProductCardDto> products)
        {
            if (!isApply)
                return products;

            return products
                    .Where(p => p.StoredQuantity > 0);
        }

        public IQueryable<ProductCardDto> ApplySorting(string sortColumn, IQueryable<ProductCardDto> products)
            => sortColumn switch
            {
                "supplier" => products.OrderBy(p => p.SupplierName),
                "price" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                _ => products.OrderBy(p => p.ProductName)
            };

        public Product ConvertDtoToProduct(ProductDto productDto)
            => new Product()
            {
                ProductId = productDto.ProductId,
                ProductName = productDto.ProductName,
                ProductCode = productDto.ProductCode,
                UnitId = productDto.UnitId,
                Price = productDto.Price,
                SupplierId = productDto.SupplierId,
                ManufacturerId = productDto.ManufacturerId,
                CategoryId = productDto.CategoryId,
                Discount = productDto.Discount,
                StoredQuantity = productDto.StoredQuantity,
                Description = productDto.Description,
                Photo = productDto.Photo
            };

        public bool ProductExists(int id) 
            => _context.Products.Any(e => e.ProductId == id);

        public bool UnitExists(int id)
            => _context.Units.Any(e => e.UnitId == id);

        public bool ManufacturerExists(int id)
            => _context.Manufacturers.Any(e => e.ManufacturerId == id);

        public bool SupplierExists(int id)
            => _context.Suppliers.Any(e => e.SupplierId == id);

        public bool CategorieExists(int id)
            => _context.Categories.Any(e => e.CategoryId == id);
    }
}
