using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;

namespace StoreLib.Services
{
    /// <summary>
    /// Сервис для управления заказами в системе магазина.
    /// Обеспечивает создание, изменение и получение информации о заказах,
    /// включая формирование составов заказов и управление их статусами.
    /// </summary>
    public class OrderService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        /// <summary>
        /// Изменяет информацию о существующем заказе.
        /// </summary>
        /// <param name="order">Объект OrderInfo с обновленными данными заказа</param>
        public async Task ChangeOrderInfo(OrderInfo order)
        {
            // переданный объект помечается как изменяемый
            _context.Entry(order).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Создает новую запись о заказе для пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Созданный объект OrderInfo</returns>
        public async Task<OrderInfo> CreateOrderInfoAsync(int userId)
        {
            var random = new Random();
            // генерация кода выдачи
            short pickupCode = Convert.ToInt16(random.Next(100, 1000));

            var order = new OrderInfo
            {
                UserId = userId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                DeliveryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                ReceiveCode = pickupCode,
                StatusId = _context.Statuses
                    .FirstOrDefault(s => s.Name == "Новый").StatusId //Созданный заказ всегда имеет статус "Новый". FirstOrDefault - чтобы брать id из БД
            };

            // Добавление созданного заказа в контекст и сохранение изменений
            _context.OrdersInfo.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        /// <summary>
        /// Получает список заказов с возможностью полученяи заказов по логину (для конкретного пользователя).
        /// </summary>
        /// <param name="Login">Логин пользователя (опционально)</param>
        /// <returns>
        /// Список объектов OrderViewDto, если передан логин - только для пользователя с этим логином
        /// </returns>
        public async Task<List<OrderViewDto>?> GetOrdersAsync(string Login = null)
        {
            // создание запроса на все заказы (запрос нужен для последующих применений LINQ)
            var query = _context.OrdersCompound
                .Include(o => o.OrderInfo) // Загрузка связанных свойств (инфориация о заказе)
                .ThenInclude(o => o.User) // Загрузка связанных свойств связанного свойства (пользователи, которые оформили заказ)
                .Include(o => o.Product)
                .AsQueryable();

            // если передан логин, взять только заказы по этому логину
            if (!string.IsNullOrEmpty(Login))
                query = query.Where(o => o.OrderInfo.User.Login == Login);

            // выбираются только необходимые поля для оптимизации
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
            // Заказа группируется по свойству OrderInfoId дял создания столбца с сосавом заказа 
            var groups = orderItems.GroupBy(o => o.OrderInfoId);

            var result = new List<OrderViewDto>();

            foreach (var group in groups)
            {
                // первый элемент заказа для получения общей информации о заказе
                var firstItem = group.First();

                // Словарь - Название товара: Общее количество в заказе
                var compoundDict = group
                    .GroupBy(i => i.ProductName)
                    .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));
                //Словарь записывается в строку формата "Товар1:Количество1; Товар2:Количество2 итд"
                var compoundString = string.Join("; ",
                    compoundDict.Select(v => $"{v.Key}:{v.Value}"));

                // Вычисляется общая стоимость заказа (сумма цен предметов на момент заказа умноженная на количество этих товаров в заказе)
                var totalPrice = group.Sum(item => item.OrderedPrice * item.Quantity);

                //добавление созданного заказа в результат
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

            // заказ сортируется по убыванию даты заказа, чтобы сначало отображались последние созданные заказы
            return result.OrderByDescending(o => o.OrderDate).ToList();
        }

        /// <summary>
        /// Создаёт новый состав заказа
        /// </summary>
        /// <param name="orderId">Идентификатор информации о заказе</param>
        /// <param name="product">DTO товара с карточки</param>
        /// <param name="quantity">Количество товара (по умолчанию 1)</param>
        /// <returns>Созданная запись OrderCompound</returns>
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

        /// <summary>
        /// Изменяет статус заказа (если передать null параметры, заказ будет доставлен)
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="deliveryDate">Новая дата доставки (если null - устанавливается текущая дата)</param>
        /// <param name="statusName">Название нового статуса (по умолчанию "Завершен")</param>
        /// <returns>Обновлённый заказ</returns>
        public async Task<OrderInfo>? ChangeOrderStatus(int id, DateOnly? deliveryDate = null, string statusName = "Завершен")
        {
            var order = await _context.OrdersInfo
                .FirstOrDefaultAsync(o => o.OrderInfoId == id);

            if (order is null)
                return null;

            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == statusName);

            if (status is null)
                return null;

            order.StatusId = status.StatusId;
            order.DeliveryDate = deliveryDate
                ??= DateOnly.FromDateTime(DateTime.Now); // deliveryDate = null нужно для значения по умолчанию (Параметр должен быть константой времени компиляции)

            return order;
        }

        /// <summary>
        /// Проверяет существование заказа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns>true, если заказ с указанным id существует в БД</returns>
        public bool OrderExists(int id)
            => _context.OrdersInfo.Any(e => e.OrderInfoId == id);
    }
}
