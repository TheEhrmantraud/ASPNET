using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class PatchProductDTO
    {
        [Range(1, int.MaxValue)]
        public int? CategoryId { get; set; }

        [StringLength(200)]
        public string? ProductName { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, 100000000)]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        public bool? IsAvailable { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }
    }
}
