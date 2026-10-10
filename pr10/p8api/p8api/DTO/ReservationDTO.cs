namespace p8api.DTOs
{
    public class ReservationDTO
    {
        public int IdReservation { get; set; }
        public int Quantity { get; set; }
        public DateTime ReservedDate { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public bool IsActive { get; set; }
        public UserShortDTO User { get; set; } = null!;
        public ProductShortDTO Product { get; set; } = null!;
    }
}
