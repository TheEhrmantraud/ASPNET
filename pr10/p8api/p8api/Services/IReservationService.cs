using p8api.DTOs;

namespace p8api.Services
{
    public interface IReservationService
    {
        Task<PagedResult<ReservationDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct);
        Task<ReservationDTO?> GetByIdAsync(int id, CancellationToken ct);
        Task<ReservationDTO> ReserveAsync(CreateReservationDTO dto, CancellationToken ct);
        Task<ReservationDTO?> CancelAsync(int id, CancellationToken ct);
    }
}
