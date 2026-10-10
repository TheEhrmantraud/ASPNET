using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class PatchUserDTO
    {
        [Range(1, int.MaxValue)]
        public int? RoleId { get; set; }

        [StringLength(50)]
        public string? Login { get; set; }

        [StringLength(255)]
        public string? PasswordHash { get; set; }

        [StringLength(40)]
        public string? Surname { get; set; }

        [StringLength(30)]
        public string? NameUser { get; set; }

        [StringLength(30)]
        public string? MiddleName { get; set; }

        [StringLength(11)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }
    }
}
