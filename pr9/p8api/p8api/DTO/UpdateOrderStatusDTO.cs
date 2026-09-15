using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class UpdateOrderStatusDTO
    {
        [Required]
        [StringLength(50)]
        public string StatusName { get; set; } = null!;
    }
}
