namespace StoreLib.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public DateOnly OrderDate { get; set; }

        public DateOnly DeliveryDate { get; set; }

        public int UserId { get; set; }

        public short ReceiveCode { get; set; }

        public byte StatusId { get; set; }
    }
}
