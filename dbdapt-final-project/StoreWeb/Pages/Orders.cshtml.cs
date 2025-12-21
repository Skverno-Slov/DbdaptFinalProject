using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;
using StoreLib.Services;

namespace StoreWeb.Pages
{
    public class OrdersModel(StoreDbContext context) : PageModel
    {
        private readonly OrderService _orderService = new(context);
        private readonly UserService _userService = new(context);

        public IList<OrderViewDto> OrderInfo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var stringId = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("Role");
            var orders = new List<OrderViewDto>();

            try
            {
                if (!Int32.TryParse(stringId, out int userId) || String.IsNullOrWhiteSpace(stringId))
                    throw new ArgumentException("Пользователь не найден");

                var user = await _userService.GetUserAsync(userId)
                    ?? throw new ArgumentException("Пользователь не найден");
                if(role == "Администратор" || role == "Менеджер")
                    orders = await _orderService.GetOrdersAsync();
                else
                    orders = await _orderService.GetOrdersAsync(user.Login);

                if(orders is null)
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
