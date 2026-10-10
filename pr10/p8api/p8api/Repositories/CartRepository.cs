using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ShopSystemContext context) : base(context)
        {
        }

        protected override IQueryable<Cart> Query()
        {
            return Set
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(i => i.Product);
        }
    }
}
