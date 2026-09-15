using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class UpdateOrderDTO
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Range(1, int.MaxValue)]
        public int OrderStatusId { get; set; }

        [Range(0, 100000000)]
        public decimal TotalAmount { get; set; }

        [StringLength(500)]
        public string? DeliveryAddress { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
