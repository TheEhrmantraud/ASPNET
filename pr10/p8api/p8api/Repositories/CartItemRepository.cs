using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ShopSystemContext context) : base(context)
        {
        }

        protected override IQueryable<CartItem> Query()
        {
            return Set
                .Include(i => i.Product);
        }
    }
}
