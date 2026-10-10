namespace p8api.DTOs
{
    public class ProductShortDTO
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
