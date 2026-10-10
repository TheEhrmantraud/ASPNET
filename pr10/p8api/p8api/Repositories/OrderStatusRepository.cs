using p8api.Models;

namespace p8api.Repositories
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(ShopSystemContext context) : base(context)
        {
        }
    }
}
