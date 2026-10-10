using p8api.Models;

namespace p8api.Repositories
{
    public interface IReservationRepository : IRepository<ProductReservation>
    {
        Task<ProductReservation?> GetActiveByIdAsync(int id, CancellationToken ct);
    }
}
