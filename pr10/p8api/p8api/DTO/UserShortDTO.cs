namespace p8api.DTOs
{
    public class UserShortDTO
    {
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }
}
