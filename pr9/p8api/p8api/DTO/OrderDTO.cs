namespace p8api.DTOs
{
    public class OrderDTO
    {
        public int IdOrder { get; set; }

        public int UserId { get; set; }

        public int OrderStatusId { get; set; }

        public decimal TotalAmount { get; set; }

        public string? DeliveryAddress { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Notes { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
