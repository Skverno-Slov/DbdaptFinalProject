using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(StoreDbContext context) : ControllerBase
    {
        private readonly StoreDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
            => await _context.Products.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] int id)
        {
            try
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == id);

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
        public async Task<ActionResult<Product>> GetProductByProductCode([FromRoute] string productCode)
        {
            try
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductCode == productCode);

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

                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                product = await ConvertDtoToProduct(productDto, product);

                _context.Entry(product).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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
                var product = new Product();

                product = await ConvertDtoToProduct(productDto, product);

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

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
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex) 
            {
                return Problem($"Непредвиденная ошибка. {ex.Message}");
            }
        }

        private async Task<Product> ConvertDtoToProduct(ProductDto productDto, Product product)
        {
            product.ProductCode = productDto.ProductCode;
            product.ProductName = productDto.ProductName;
            product.UnitId = productDto.UnitId;
            product.Price = productDto.Price;
            product.SupplierId = productDto.SupplierId;
            product.ManufacturerId = productDto.ManufacturerId;
            product.CategoryId = productDto.CategoryId;
            product.Discount = productDto.Discount;
            product.StoredQuantity = productDto.StoredQuantity;
            product.Description = productDto.Description;
            product.Photo = productDto.Photo;

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
