namespace p8api.DTOs
{
    public class CategoryDTO
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public CategoryShortDTO? ParentCategory { get; set; }
    }
}
