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

        public short ReceiveCode { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public string StatusName { get; set; } = null!;
    }
}
