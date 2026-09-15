namespace p8api.DTOs
{
    public class OrderItemDTO
    {
        public int IdOrderItem { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal PriceAtOrder { get; set; }

        public decimal? Subtotal { get; set; }
    }
}
