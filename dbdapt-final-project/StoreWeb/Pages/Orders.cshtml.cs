using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Services;

namespace StoreWeb.Pages
{
    public class OrdersModel(StoreDbContext context) : PageModel
    {
        private readonly OrderService _orderService = new(context);
        private readonly UserService _userService = new(context);

        public IList<OrderViewDto> OrderInfo { get; set; } = default!;

        public async Task OnGetAsync()
        {
            //Получение роли и id текущего пользователя
            var stringId = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("Role");
            var orders = new List<OrderViewDto>();

            try
            {
                if (!Int32.TryParse(stringId, out int userId) || String.IsNullOrWhiteSpace(stringId))
                    throw new ArgumentException("Пользователь не найден");

                var user = await _userService.GetUserAsync(userId)
                    ?? throw new ArgumentException("Пользователь не найден");
                if (role == "Администратор" || role == "Менеджер")
                    orders = await _orderService.GetOrdersAsync(); //если роль Администратор или Менеджер - выводить все заказы (не передавть логин в метод получения из библиотеки)
                else
                    orders = await _orderService.GetOrdersAsync(user.Login); // иначе заказы получаются по логину

                if (orders is null)
                    OrderInfo = new List<OrderViewDto>();
                else
                    OrderInfo = orders;
            }
            catch
            {
            }
        }
    }
}
