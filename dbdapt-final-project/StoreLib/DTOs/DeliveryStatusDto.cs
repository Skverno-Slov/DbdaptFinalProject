using StoreLib.Models;

namespace StoreLib.DTOs
{
    // DTO класс для тела запоса метода PatchOrderStatus
    public class DeliveryStatusDto
    {
        public string? StatusName { get; set; }
        public DateOnly? DeliveryDate { get; set; }
    }
}
