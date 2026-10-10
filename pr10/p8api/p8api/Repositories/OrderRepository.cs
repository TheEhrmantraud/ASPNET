using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ShopSystemContext context) : base(context)
        {
        }

        protected override IQueryable<Order> Query()
        {
            return Set
                .Include(o => o.User)
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product);
        }
    }
}
