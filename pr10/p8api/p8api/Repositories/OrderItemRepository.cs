using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ShopSystemContext context) : base(context)
        {
        }

        protected override IQueryable<OrderItem> Query()
        {
            return Set
                .Include(i => i.Product);
        }
    }
}
