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
    public class OrdersController(StoreDbContext context) : ControllerBase
    {
        private readonly StoreDbContext _context = context;

        //GET: api/Orders/5
        [HttpGet("{Login}")]
        [Authorize]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByUserLogin(string Login)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(u => u.User)
                    .Where(o => o.User.Login == Login)
                    .Select(o => new OrderDto
                    {
                        OrderId = o.OrderId,
                        OrderDate = o.OrderDate,
                        DeliveryDate = o.DeliveryDate,
                        UserId = o.UserId,
                        ReceiveCode = o.ReceiveCode,
                        StatusId = o.StatusId,
                    })
                    .ToListAsync();

                if (orders.Count == 0)
                    return NotFound();

                return orders;
            }
            catch (Exception ex)
            {
                return Problem($"Непредвиденная ошибка. {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Администратор,Менеджер")]
        public async Task<IActionResult> PatchOrderStatus(int id, DeliveryStatusDto deliveryStatus)
        {
            try
            {
                deliveryStatus.Status ??= await _context.Statuses
                    .FirstOrDefaultAsync(s => s.Name == "Завершен");

                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                if (order == null)
                    return NotFound();

                order.StatusId = deliveryStatus.Status.StatusId;
                order.DeliveryDate = deliveryStatus.DeliveryDate
                    ??= DateOnly.FromDateTime(DateTime.Now);

                _context.Entry(order).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
