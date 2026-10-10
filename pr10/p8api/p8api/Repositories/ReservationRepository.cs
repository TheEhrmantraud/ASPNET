using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class ReservationRepository : Repository<ProductReservation>, IReservationRepository
    {
        public ReservationRepository(ShopSystemContext context) : base(context)
        {
        }

        protected override IQueryable<ProductReservation> Query()
        {
            return Set
                .Include(r => r.User)
                .Include(r => r.Product);
        }

        public async Task<ProductReservation?> GetActiveByIdAsync(int id, CancellationToken ct)
        {
            return await Query().FirstOrDefaultAsync(r => r.IdReservation == id && r.ReleasedDate == null, ct);
        }
    }
}
