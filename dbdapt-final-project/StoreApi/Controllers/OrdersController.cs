using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;
using StoreLib.Services;

namespace StoreApi.Controllers
{
    /// <summary>
    /// Контроллер для управления заказами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(StoreDbContext context) : ControllerBase
    {
        private readonly OrderService _orderService = new(context);

        /// <summary>
        /// Получает список заказов по логину
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <returns>Список заказов пользователя OrderViewDto</returns>
        /// <response code="200">Успешно возвращен список заказов</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="404">Заказы для указанного пользователя не найдены</response>
        /// <response code="500">Непредвиденная ошибка</response>
        [HttpGet("{Login}")]
        [Authorize] //Для использования требуется авторизация
        public async Task<ActionResult<List<OrderViewDto>>> GetOrdersByUserLogin(string Login)
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync(Login);

                if (orders is null)
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

        /// <summary>
        /// Изменяет статус заказа и дату доставки
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="deliveryStatus">DTO с данными для обновления статуса заказа</param>
        /// <response code="204">Статус заказа успешно обновлен</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Недостаточно прав (требуется роль Администратор или Менеджер)</response>
        /// <response code="404">Заказ с указанным id не найден</response>
        /// <response code="500">Непредвиденная ошибка</response>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Администратор,Менеджер")] //для использования требуется авторизация с ролью Администратор или Менеджер
        public async Task<IActionResult> PatchOrderStatus(int id, DeliveryStatusDto deliveryStatus)
        {
            try
            {
                var order = new OrderInfo();

                if (deliveryStatus.StatusName is null) // если передан StatusName (Не null в запросе) использовать статус по умолчанию
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
