using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class PatchCategoryDTO
    {
        [StringLength(100)]
        public string? CategoryName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
