using p8api.Models;

namespace p8api.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ShopSystemContext context) : base(context)
        {
        }
    }
}
