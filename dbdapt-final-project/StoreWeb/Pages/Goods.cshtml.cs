using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Services;

namespace StoreWeb.Pages
{
    public class GoodsModel(StoreDbContext context) : PageModel
    {
        private readonly ProductService _productService = new(context);
        private readonly OrderService _orderService = new(context);

        [BindProperty(SupportsGet = true)] //привязка свойства, поддерживающее метод get и post
        public string? ProductDescription { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MaxPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedManufacturer { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDiscounted { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsInStock { get; set; }

        [TempData] //Сохраняет данные только на время 1 запроса
        public string? OrderMessage { get; set; }

        [TempData]
        public bool IsOrderSuccess { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; }

        public IList<ProductCardDto> Product { get; set; } = default!; //список товаров

        //метод для создания заказа
        public async Task<IActionResult> OnPostCreateOrderAsync(int productId)
        {
            var stringId = HttpContext.Session.GetString("UserId"); //получение id пользователя для создания заказа для него

            var product = await _productService.GetProductCard(productId);

            try
            {
                if (!Int32.TryParse(stringId, out int userId) || String.IsNullOrWhiteSpace(stringId))
                    throw new ArgumentException("Пользователь не найден");

                var order = await _orderService.CreateOrderInfoAsync(userId);

                var changedProduct = await _productService.GetProductAsync(productId);
                changedProduct.StoredQuantity--; //Уменбшение кол-л товара на складе, после заказа
                await _productService.ChangeProductAsync(changedProduct);

                await _orderService.CreateOrderCompoundAsync(order.OrderInfoId, product);

                //вывод сообщения о успешном заказе
                OrderMessage = $" Заказ успешно создан.  Код для получения: {order.ReceiveCode}.  Дата доставки: {order.DeliveryDate:dd.MM.yyyy}";
                IsOrderSuccess = true;
            }
            catch (Exception ex)
            {
                //вывод сообщения о не успешном заказе
                OrderMessage = $"Ошибка при создании заказа: {ex.Message}";
                IsOrderSuccess = false;
            }

            return RedirectToPage();
        }

        public async Task OnGetAsync() //Выполняется при загрузке страницы
        {
            //Загрузка данных о производителе из бд
            var manufacturers = await _productService.GetManufacturersAsync();
            var manufacturerList = new List<SelectListItem>
            {
                new() { Value = "", Text = "Все производители" } //Добавление "Все производители" в варианты
            };
            // заполнене списка SelectListItem для ViewData Value- значение; Text-то, что отображается (свойства SelectListItem)
            manufacturerList.AddRange(manufacturers.Select(m =>
                new SelectListItem { Value = m.Name, Text = m.Name }));

            ViewData["Manufacturers"] = manufacturerList; //здесь задается источник данных для списка производителей

            //применение фильтров -> сортировки
            var products = _productService.GetProductCards();

            products = _productService.ApplyDescriptionFilter(ProductDescription, products);
            products = _productService.ApplyManufacturerFilter(SelectedManufacturer, products);
            products = _productService.ApplyMaxPriceFilter(MaxPrice, products);
            products = _productService.ApplyInStockFilter(IsInStock, products);
            products = _productService.ApplyDiscountedFilter(IsDiscounted, products);

            products = _productService.ApplySorting(SortColumn, products);

            Product = await products.ToListAsync();
        }
    }
}
