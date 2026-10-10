namespace p8api.Models;

public partial class ProductReservation
{
    public int IdReservation { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime ReservedDate { get; set; }

    public DateTime? ReleasedDate { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
