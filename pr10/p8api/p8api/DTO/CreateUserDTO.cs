using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class CreateUserDTO
    {
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string Login { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [StringLength(40)]
        public string Surname { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string NameUser { get; set; } = null!;

        [StringLength(30)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
