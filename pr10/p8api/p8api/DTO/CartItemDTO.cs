namespace p8api.DTOs
{
    public class CartItemDTO
    {
        public int IdCartItem { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }
        public ProductShortDTO Product { get; set; } = null!;
    }
}
