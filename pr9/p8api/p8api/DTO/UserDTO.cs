namespace p8api.DTOs
{
    public class UserDTO
    {
        public int IdUser { get; set; }

        public int RoleId { get; set; }

        public string Login { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string NameUser { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
