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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
            => await _productService.GetProductsAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] int id)
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

        // PUT: api/Products/5
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

                if (product.ProductCode.Length > 6)
                    return BadRequest("Артикул слишком длинный");

                if (product.ProductCode.Length < 6)
                    return BadRequest("Артикул слишком короткий");

                if (product.Price <= 0)
                    return BadRequest("Цена не может быть отрицательной или 0");

                if (product.StoredQuantity < 0)
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

        // POST: api/Products
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

                if (product.ProductCode.Length > 6)
                    return BadRequest("Артикул слишком длинный");

                if (product.ProductCode.Length < 6)
                    return BadRequest("Артикул слишком короткий");

                if (product.Price <= 0)
                    return BadRequest("Цена должна быть болше 0");

                if (product.StoredQuantity < 0)
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

        // DELETE: api/Products/5
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
