namespace p8api.DTOs
{
    public class CartDTO
    {
        public int IdCart { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public UserShortDTO User { get; set; } = null!;
        public List<CartItemDTO> CartItems { get; set; } = new();
    }
}
