using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class CreateOrderItemDTO
    {
        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }

        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, 100000000)]
        public decimal PriceAtOrder { get; set; }
    }
}
