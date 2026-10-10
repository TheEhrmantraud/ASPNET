using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class UpdateCartItemDTO
    {
        [Range(1, int.MaxValue)]
        public int CartId { get; set; }

        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
