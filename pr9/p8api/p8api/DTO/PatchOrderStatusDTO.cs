using System.ComponentModel.DataAnnotations;

namespace p8api.DTOs
{
    public class PatchOrderStatusDTO
    {
        [StringLength(50)]
        public string? StatusName { get; set; }
    }
}
