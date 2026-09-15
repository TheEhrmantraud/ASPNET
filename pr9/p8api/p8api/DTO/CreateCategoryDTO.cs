using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class CreateCategoryDTO
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
