using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;

namespace StoreLib.Services
{
    public class OrderService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        public async Task ChangeOrderInfo(OrderInfo order)
        {
            _context.Entry(order).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<OrderInfo> CreateOrderInfoAsync(int userId)
        {
            var random = new Random();
            short pickupCode = Convert.ToInt16(random.Next(100, 1000));

            var order = new OrderInfo
            {
                UserId = userId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                DeliveryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                ReceiveCode = pickupCode,
                StatusId = _context.Statuses
                    .FirstOrDefault(s => s.Name == "Новый").StatusId
            };

            _context.OrdersInfo.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<OrderViewDto>?> GetOrdersAsync(string Login = null)
        {
            var query = _context.OrdersCompound
                .Include(o => o.OrderInfo)
                .ThenInclude(o => o.User)
                .Include(o => o.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Login))
                query = query.Where(o => o.OrderInfo.User.Login == Login);

            var orderItems = await query
                .Select(o => new
                {
                    o.OrderInfoId,
                    o.OrderInfo.OrderDate,
                    o.Product.ProductName,
                    o.Quantity,
                    o.OrderedPrice,
                    o.OrderInfo.DeliveryDate,
                    o.OrderInfo.ReceiveCode,
                    o.OrderInfo.Status.Name,
                })
                .ToListAsync();

            if (!orderItems.Any())
                return null;

            var groups = orderItems.GroupBy(o => o.OrderInfoId);

            var result = new List<OrderViewDto>();

            foreach (var group in groups)
            {
                var firstItem = group.First();

                var compoundDict = group
                    .GroupBy(i => i.ProductName)
                    .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

                var compoundString = string.Join("; ",
                    compoundDict.Select(v => $"{v.Key}:{v.Value}"));

                var totalPrice = group.Sum(item => item.OrderedPrice * item.Quantity);

                result.Add(new OrderViewDto
                {
                    OrderInfoId = group.Key,
                    OrderDate = firstItem.OrderDate,
                    Compound = compoundString,
                    FinalPrice = totalPrice,
                    ReceiveCode = firstItem.ReceiveCode,
                    DeliveryDate = firstItem.DeliveryDate,
                    StatusName = firstItem.Name
                });
            }

            return result.OrderByDescending(o => o.OrderDate).ToList();
        }

        public async Task<OrderCompound> CreateOrderCompoundAsync(int orederId,
                                                ProductCardDto product,
                                                int quantity = 1)
        {
            var item = new OrderCompound()
            {
                OrderInfoId = orederId,
                ProductId = product.ProductId,
                Quantity = quantity,
                OrderedPrice = product.DiscountedPrice,
            };

            _context.OrdersCompound.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<OrderInfo>? ChangeOrderStatus(int id, DateOnly? deliveryDate = null, string statusName = "Завершен")
        {
            var order = await _context.OrdersInfo
                .FirstOrDefaultAsync(o => o.OrderInfoId == id);
            
            if(order is null)
                return null;

            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == statusName);

            if(status is null)
                return null;

            order.StatusId = status.StatusId;
            order.DeliveryDate = deliveryDate
                ??= DateOnly.FromDateTime(DateTime.Now);

            return order;
        }

        public bool OrderExists(int id)
            => _context.OrdersInfo.Any(e => e.OrderInfoId == id);
    }
}
