using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class CreateRoleDTO
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = null!;

        [StringLength(200)]
        public string? Description { get; set; }
    }
}
