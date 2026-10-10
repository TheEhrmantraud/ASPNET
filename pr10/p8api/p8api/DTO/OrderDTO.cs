namespace p8api.DTOs
{
    public class OrderDTO
    {
        public int IdOrder { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public UserShortDTO User { get; set; } = null!;
        public OrderStatusDTO OrderStatus { get; set; } = null!;
        public List<OrderItemDTO> OrderItems { get; set; } = new();
    }
}
