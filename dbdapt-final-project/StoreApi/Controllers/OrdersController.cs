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
    public class OrdersController(StoreDbContext context) : ControllerBase
    {
        private readonly OrderService _orderService = new(context);

        //GET: api/Orders/5
        [HttpGet("{Login}")]
        [Authorize]
        public async Task<ActionResult<List<OrderViewDto>>> GetOrdersByUserLogin(string Login)
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync(Login);

                if (orders == null)
                    return NotFound();

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
                var order = new OrderInfo();

                if (deliveryStatus.StatusName is null)
                    order = await _orderService.ChangeOrderStatus(id,
                                                            deliveryStatus.DeliveryDate);
                else
                    order = await _orderService.ChangeOrderStatus(id,
                                                            deliveryStatus.DeliveryDate,
                                                            deliveryStatus.StatusName);

                if (order is null)
                    return NotFound();

                _orderService.ChangeOrderInfo(order);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_orderService.OrderExists(id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                return Problem($"Непредвиденная ошибка. {ex.Message}");
            }
        }
    }
}
