using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class UpdateCartDTO
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
