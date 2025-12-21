using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;
using StoreLib.Services;

namespace StoreWeb.Pages
{
    public class GoodsModel(StoreDbContext context) : PageModel
    {
        private readonly ProductService _productService = new(context);
        private readonly ManufacturerService _manufacturerService = new(context);

        [BindProperty(SupportsGet = true)]
        public string? ProductDescription { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MaxPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedManufacturer { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDiscounted { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsInStock { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; }

        public IList<ProductCardDto> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var manufacturers = await _manufacturerService.GetManufacturersAsync();
            var manufacturerList = new List<SelectListItem>
            {
                new() { Value = "", Text = "Все производители" }
            };
            manufacturerList.AddRange(manufacturers.Select(m =>
                new SelectListItem { Value = m.Name, Text = m.Name }));

            ViewData["Manufacturers"] = manufacturerList;

            //Product?.Clear();

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
