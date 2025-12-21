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

                if (product == null)
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

                if (product == null)
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

                if (product == null)
                    return NotFound();

                if (!_productService.IsDiscountValid(product.Discount))
                    return BadRequest("Слишком блоьшая скидка");

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

                if (!_productService.IsDiscountValid(product.Discount))
                    return BadRequest("Слишком блоьшая скидка");

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
                if (product == null)
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
