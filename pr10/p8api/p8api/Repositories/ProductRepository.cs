using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ShopSystemContext context) : base(context)
        {
        }

        protected override IQueryable<Product> Query()
        {
            return Set
                .Include(p => p.Category);
        }
    }
}
