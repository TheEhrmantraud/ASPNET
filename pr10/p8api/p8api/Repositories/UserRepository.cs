using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ShopSystemContext context) : base(context)
        {
        }

        protected override IQueryable<User> Query()
        {
            return Set
                .Include(u => u.Role);
        }
    }
}
