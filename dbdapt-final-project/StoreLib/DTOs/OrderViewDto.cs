using StoreLib.Models;
using System.ComponentModel.DataAnnotations;

namespace StoreLib.DTOs
{
    public class OrderViewDto
    {
        [Display(Name = "Номер заказа")]
        public int OrderInfoId { get; set; }

        [Display(Name = "Дата заказа")]
        public DateOnly OrderDate { get; set; }

        [Display(Name = "Состав")]
        public string Compound { get; set; } = null!;

        [Display(Name = "Итоговая стоимость")]
        public decimal FinalPrice { get; set; }

        public string ProductName { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal OrderedPrice { get; set; }
    }
}
