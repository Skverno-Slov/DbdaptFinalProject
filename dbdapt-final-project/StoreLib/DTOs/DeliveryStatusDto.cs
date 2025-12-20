using StoreLib.Models;

namespace StoreLib.DTOs
{
    // DTO класс для тела запоса метода PatchOrderStatus
    public class DeliveryStatusDto
    {
        public Status? Status { get; set; }
        public DateOnly? DeliveryDate { get; set; }
    }
}
