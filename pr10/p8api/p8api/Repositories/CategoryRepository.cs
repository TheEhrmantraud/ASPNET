using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShopSystemContext context) : base(context)
        {
        }

        protected override IQueryable<Category> Query()
        {
            return Set
                .Include(c => c.ParentCategory);
        }
    }
}
