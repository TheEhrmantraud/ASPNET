using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class CreateCartDTO
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
