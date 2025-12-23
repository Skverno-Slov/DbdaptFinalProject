using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;

namespace StoreLib.Services
{
    /// <summary>
    /// Сервис для управления товарами и связаннными данными (поставщик, производитель, еденицы имзерения, категория)
    /// </summary>
    public class ProductService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        /// <summary>
        /// Получает список всех товаров из базы данных.
        /// </summary>
        /// <returns>Список товаров из БД</returns>
        public async Task<List<Product>> GetProductsAsync()
            => await _context.Products.ToListAsync();

        /// <summary>
        /// Получает товар по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Объект Product или null, если товар не найден.</returns>
        public async Task<Product>? GetProductAsync(int id)
            => await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);


        /// <summary>
        /// Получает товар по его артикулу (для 3-ого задния)
        /// </summary>
        /// <param name="code">Артикул товара.</param>
        /// <returns>Объект Product или null, если товар не найден.</returns>
        public async Task<Product>? GetProductByCodeAsync(string code)
            => await _context.Products.FirstOrDefaultAsync(p => p.ProductCode == code);

        /// <summary>
        /// Обновляет информацию о существующем товаре.
        /// </summary>
        /// <param name="product">Объект товара с обновленными данными.</param>
        public async Task ChangeProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Добавляет новый товар в БД.
        /// </summary>
        /// <param name="product">Товар для добавления.</param>
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет товар из БД.
        /// </summary>
        /// <param name="product">Товар для удаления.</param>
        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Проверяет корректность скидки
        /// </summary>
        /// <param name="discount">Значение скидки</param>
        /// <returns>true, если скидка находится в диапазоне 0-99</returns>
        public bool CheckDiscountValid(byte discount)
        {
            if (discount >= 100 || discount < 0)
                return false;
            return true;
        }

        /// <summary>
        /// Получает карточку товара по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>DTO карточки товара</returns>
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

        //Слдующте методы возвращают IQueryable<ProductCardDto> для последовательного применения фильтров и сортировок

        /// <summary>
        /// Получает все карточки товаров в виде запроса
        /// </summary>
        /// <returns>Запрос всех карточек товаров</returns>
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

        /// <summary>
        /// Применяет фильтр по описанию товара.
        /// </summary>
        /// <param name="description">Строка для поиска в описании. Может содержать несколько слов, разделенных пробелами</param>
        /// <param name="products">Запрос с товарами</param>
        /// <returns>Отфильтрованный по части описания запрос с товарами</returns>
        public IQueryable<ProductCardDto> ApplyDescriptionFilter(string? description,
                                                                 IQueryable<ProductCardDto> products)
        {

            // если описание пустое нет смысла выплнять дальше
            if (String.IsNullOrWhiteSpace(description))
                return products;

            // полусение отдельных слов в описании
            var filters = description.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // перебирает все полученные слова и ищет совпадение с каждым из них
            foreach (var filter in filters)
            {
                products = products
                    .Where(p => p.Description.ToLower() // ToLower() применяется непосредственно к описанию товара и к фильтру (одному из слов строки поиска) для игнорирования регистра
                    .Contains(filter.ToLower()));
            }

            // возвращаются те товары, описание которых содержит каждый фильтер
            return products;
        }

        /// <summary>
        /// Применяет фильтр по производителю
        /// </summary>
        /// <param name="manufacturerName">Название производителя</param>
        /// <param name="products">Запрос</param>
        /// <returns>Запрос только с выбраным производителем товаров</returns>
        public IQueryable<ProductCardDto> ApplyManufacturerFilter(string manufacturerName,
                                                            IQueryable<ProductCardDto> products)
        {
            if (string.IsNullOrWhiteSpace(manufacturerName))
                return products; // Здесь Фильтер не применяется если переданно пустое значение (Т.Е. выбранн пункт все производители). В теории может пригодится для локализации

            return products
                .Where(p => p.ManufacturerName == manufacturerName);
        }

        /// <summary>
        /// Применяет фильтр по максимальной цене товара (с учётом скидки)
        /// </summary>
        /// <param name="maxPrice">Максимальная цена</param>
        /// <param name="products">Запрос</param>
        /// <returns>Запрос, где цена всех элементов не превышает заданную величину</returns>
        public IQueryable<ProductCardDto> ApplyMaxPriceFilter(decimal? maxPrice,
                                                              IQueryable<ProductCardDto> products)
        {
            if (!maxPrice.HasValue || maxPrice <= 0)
                return products; //HasValue - на случай, если поле ввода было пустым

            return products
            .Where(p => (p.Discount > 0) //Если есть скидка, учиитывать и её
                ? p.Price * (100 - p.Discount) / 100 <= maxPrice.Value
                : p.Price <= maxPrice);
        }

        /// <summary>
        /// Применяет фильтр по наличию скидки
        /// </summary>
        /// <param name="isApply">true - показывать только товары со скидкой</param>
        /// <param name="products">Запрос</param>
        /// <returns>Запрос, где все товары содержат скидук</returns>
        public IQueryable<ProductCardDto> ApplyDiscountedFilter(bool isApply, IQueryable<ProductCardDto> products)
        {
            if (!isApply)
                return products;

            return products
                    .Where(p => p.Discount > 0);
        }

        /// <summary>
        /// Применяет фильтр по наличию товара на складе
        /// </summary>
        /// <param name="isApply">true - показывать только товары в наличии</param>
        /// <param name="products">Запрос</param>
        /// <returns>Запрос, где все товары есть на складе</returns>
        public IQueryable<ProductCardDto> ApplyInStockFilter(bool isApply, IQueryable<ProductCardDto> products)
        {
            if (!isApply)
                return products;

            return products
                    .Where(p => p.StoredQuantity > 0);
        }

        /// <summary>
        /// Применяет сортировку к списку товаров
        /// </summary>
        /// <param name="sortColumn">Колонка для сортировки</param>
        /// <param name="products">Запрос</param>
        /// <returns>Отсортированный запрос товаров</returns>
        public IQueryable<ProductCardDto> ApplySorting(string sortColumn, IQueryable<ProductCardDto> products)
            => sortColumn switch
            {
                "По поставщику" => products.OrderBy(p => p.SupplierName),
                "По цене (дешевые)" => products.OrderBy(p => p.Price),
                "По цене (дорогие)" => products.OrderByDescending(p => p.Price),
                _ => products.OrderBy(p => p.ProductName)
            }; //переберает возможные значения переданного значения, применяет сортирвку по соответсвующему столбцу. Если не находит совпадений, сортирует по названию

        /// <summary>
        /// Преобразует ProductDto товара в Product. Нужен для некоторых запросов в API
        /// </summary>
        /// <param name="productDto">ProductDto</param>
        /// <returns>Product, заполненная данными из ProductDto.</returns>
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

        /// <summary>
        /// Проверяет существование товара с указанным артикулом.
        /// </summary>
        /// <param name="productCode">Артикул товара</param>
        /// <returns>true, если товар с таким артикулом существует</returns>
        public bool ProductCodeExists(string productCode)
            => _context.Products.Any(p => p.ProductCode == productCode);

        /// <summary>
        /// Проверяет существование товара по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>true, если товар существует</returns>
        public bool ProductExists(int id)
            => _context.Products.Any(p => p.ProductId == id);

        // Я решил оставить следующте методы здесь, поскольку они используются совместно с товаром (нет смысла разделять)

        /// <summary>
        /// Получает список всех единиц измерения товаров
        /// </summary>
        /// <returns>Список единиц измерения</returns>
        public async Task<List<Unit>> GetUnitsAsync()
            => await _context.Units
            .ToListAsync();

        /// <summary>
        /// Проверяет существование единицы измерения по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>true, если единица измерения существует</returns>
        public bool UnitExists(int id)
            => _context.Units.Any(u => u.UnitId == id);

        /// <summary>
        /// Получает список всех производителей
        /// </summary>
        /// <returns>Список производителей</returns>
        public async Task<List<Manufacturer>> GetManufacturersAsync()
            => await _context.Manufacturers
            .ToListAsync();

        /// <summary>
        /// Проверяет существование производителя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>true, если производитель существует</returns>
        public bool ManufacturerExists(int id)
            => _context.Manufacturers.Any(m => m.ManufacturerId == id);

        /// <summary>
        /// Получает список всех поставщиков
        /// </summary>
        /// <returns>Список поставщиков</returns>
        public async Task<List<Supplier>> GetSuppliersAsync()
            => await _context.Suppliers
            .ToListAsync();

        /// <summary>
        /// Проверяет существование поставщика по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>true, если поставщик существует</returns>
        public bool SupplierExists(int id)
            => _context.Suppliers.Any(s => s.SupplierId == id);

        /// <summary>
        /// Получает список всех категорий товаров
        /// </summary>
        /// <returns>Список категорий</returns>
        public async Task<List<Category>> GetCategoriesAsync()
            => await _context.Categories
            .ToListAsync();

        /// <summary>
        /// Проверяет существование категории по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>true, если категория существует</returns>
        public bool CategorieExists(int id)
            => _context.Categories.Any(c => c.CategoryId == id);
    }
}
