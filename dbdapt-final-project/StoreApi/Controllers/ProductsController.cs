using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;
using StoreLib.Services;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(StoreDbContext context) : ControllerBase
    {
        private readonly ProductService _productService = new(context);

        private const byte ProductCodeLenth = 6;
        private const byte MinimumPriseValue = 0;
        private const byte MinimumQuantityValue = 0;

        /// <summary>
        /// Получает список всех товаров
        /// </summary>
        /// <returns>Список всех товаров</returns>
        /// <response code="200">Список товаров успешно получен</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
            => await _productService.GetProductsAsync();

        /// <summary>
        /// Получает товар по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Объект товара</returns>
        /// <response code="200">Товар найден</response>
        /// <response code="404">Товар с указанным id не найден</response>
        /// <response code="500">Непредвиденная ошибка</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] int id) //FromRoute - значение из пути запроса -> "{id}"
        {
            try
            {
                var product = await _productService
                    .GetProductAsync(id);

                if (product is null)
                    return NotFound();

                return product;
            }
            catch (Exception ex)
            {
                return Problem($"Непредвиденная ошибка. {ex.Message}");
            }
        }

        /// <summary>
        /// Получает товар по его артикулу
        /// </summary>
        /// <param name="productCode">Артикул товара</param>
        /// <returns>Объект товара</returns>
        /// <response code="200">Товар найден</response>
        /// <response code="404">Товар с указанным артикулом не найден</response>
        /// <response code="500">Непредвиденная ошибка</response>
        [HttpGet("productCode/{productCode}")]
        public async Task<ActionResult<Product>> GetProductByCode([FromRoute] string productCode)
        {
            try
            {
                var product = await _productService
                    .GetProductByCodeAsync(productCode);

                if (product is null)
                    return NotFound();

                return product;
            }
            catch (Exception ex)
            {
                return Problem($"Непредвиденная ошибка. {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет существующий товар
        /// </summary>
        /// <param name="id">Идентификатор обновляемого товара</param>
        /// <param name="productDto">DTO с обновленными данными товара</param>
        /// <response code="204">Товар успешно обновлен</response>
        /// <response code="400">
        /// Возможные ошибки:
        /// - Несоответствие id в пути и теле
        /// - Не указано название товара
        /// - Не указан артикул
        /// - Длина артикула не равна 6 символам
        /// - Цена отрицательная или равна 0
        /// - Количество отрицательное
        /// - Некорректная скидка
        /// - Указанная единица измерения не существует
        /// - Указанный производитель не существует
        /// - Указанный поставщик не существует
        /// - Указанная категория не существует
        /// </response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Недостаточно прав</response>
        /// <response code="404">Товар с указанным id не найден</response>
        /// <response code="500">Непредвиденная ошибка</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Администратор,Менеджер")]
        public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
        {
            try
            {
                if (id != productDto.ProductId)
                    return BadRequest();
                Product product = _productService.ConvertDtoToProduct(productDto);

                if (product is null)
                    return NotFound();

                if (String.IsNullOrWhiteSpace(product.ProductName))
                    return BadRequest("Введите название");

                if (String.IsNullOrWhiteSpace(product.ProductCode))
                    return BadRequest("Введите артикул");

                if (product.ProductCode.Length > ProductCodeLenth)
                    return BadRequest("Артикул слишком длинный");

                if (product.ProductCode.Length < ProductCodeLenth)
                    return BadRequest("Артикул слишком короткий");

                if (product.Price <= MinimumPriseValue)
                    return BadRequest("Цена не может быть отрицательной или 0");

                if (product.StoredQuantity < MinimumQuantityValue)
                    return BadRequest("Количество не может быть отрицательным");

                if (!_productService.CheckDiscountValid(product.Discount))
                    return BadRequest("Некорректная скидка");

                if (!_productService.UnitExists(product.UnitId))
                    return BadRequest("Еденица измерения не существует");

                if (!_productService.ManufacturerExists(product.ManufacturerId))
                    return BadRequest("Производитель не существует");

                if (!_productService.SupplierExists(product.SupplierId))
                    return BadRequest("Поставщик не существует");

                if (!_productService.CategorieExists(product.CategoryId))
                    return BadRequest("Категория не существует");

                await _productService.ChangeProductAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_productService.ProductExists(id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                return Problem($"Непредвиденная ошибка. {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Создает новый товар
        /// </summary>
        /// <param name="productDto">DTO с данными нового товара</param>
        /// <returns>Созданный товар</returns>
        /// <response code="201">Товар успешно создан</response>
        /// <response code="400">
        /// Возможные ошибки:
        /// - Не указано название товара
        /// - Не указан артикул
        /// - Артикул уже существует
        /// - Длина артикула не равна 6 символам
        /// - Цена меньше или равна 0
        /// - Количество отрицательное
        /// - Некорректная скидка
        /// - Указанная единица измерения не существует
        /// - Указанный производитель не существует
        /// - Указанный поставщик не существует
        /// - Указанная категория не существует
        /// </response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Недостаточно прав</response>
        /// <response code="500">Непредвиденная ошибка</response>
        [HttpPost]
        [Authorize(Roles = "Администратор,Менеджер")]
        public async Task<ActionResult<Product>> PostProduct(ProductDto productDto)
        {
            try
            {
                var product = _productService.ConvertDtoToProduct(productDto);

                if (String.IsNullOrWhiteSpace(product.ProductName))
                    return BadRequest("Введите название");

                if (String.IsNullOrWhiteSpace(product.ProductCode))
                    return BadRequest("Введите артикул");

                if (_productService.ProductCodeExists(product.ProductCode))
                    return BadRequest("Артикул уже существкет");

                if (product.ProductCode.Length > ProductCodeLenth)
                    return BadRequest("Артикул слишком длинный");

                if (product.ProductCode.Length < ProductCodeLenth)
                    return BadRequest("Артикул слишком короткий");

                if (product.Price <= MinimumPriseValue)
                    return BadRequest("Цена должна быть болше 0");

                if (product.StoredQuantity < MinimumQuantityValue)
                    return BadRequest("Количество должно быть больше 0 или 0");

                if (!_productService.CheckDiscountValid(product.Discount))
                    return BadRequest("Слишком большая скидка");

                if (!_productService.UnitExists(product.UnitId))
                    return BadRequest("Еденица измерения не существует");

                if (!_productService.ManufacturerExists(product.ManufacturerId))
                    return BadRequest("Производитель не существует");

                if (!_productService.SupplierExists(product.SupplierId))
                    return BadRequest("Поставщик не существует");

                if (!_productService.CategorieExists(product.CategoryId))
                    return BadRequest("Категория не существует");

                await _productService.AddProductAsync(product);

                return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                return Problem($"Непредвиденная ошибка. {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет товар по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемого товара</param>
        /// <response code="204">Товар успешно удален</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Недостаточно прав</response>
        /// <response code="404">Товар с указанным id не найден</response>
        /// <response code="500">Непредвиденная ошибка</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Администратор,Менеджер")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);
                if (product is null)
                    return NotFound();

                await _productService.DeleteProductAsync(product);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem($"Непредвиденная ошибка. {ex.Message}");
            }
        }
    }
}
