using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class PatchRoleDTO
    {
        [StringLength(50)]
        public string? RoleName { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }
    }
}
