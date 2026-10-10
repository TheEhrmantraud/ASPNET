using Microsoft.EntityFrameworkCore;
using p8api.Models;

namespace p8api.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ShopSystemContext Context;
        protected readonly DbSet<TEntity> Set;
        private readonly string _keyName;

        public Repository(ShopSystemContext context)
        {
            Context = context;
            Set = context.Set<TEntity>();
            _keyName = context.Model.FindEntityType(typeof(TEntity))!.FindPrimaryKey()!.Properties[0].Name;
        }

        protected virtual IQueryable<TEntity> Query()
        {
            return Set;
        }

        public async Task<List<TEntity>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            return await Query()
                .AsNoTracking()
                .OrderBy(e => EF.Property<int>(e, _keyName))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct)
        {
            return await Set.CountAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await Query().FirstOrDefaultAsync(e => EF.Property<int>(e, _keyName) == id, ct);
        }

        public async Task<TEntity?> FindAsync(int id, CancellationToken ct)
        {
            return await Set.FindAsync(new object[] { id }, ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await Set.AnyAsync(e => EF.Property<int>(e, _keyName) == id, ct);
        }

        public async Task AddAsync(TEntity entity, CancellationToken ct)
        {
            await Set.AddAsync(entity, ct);
        }

        public void Remove(TEntity entity)
        {
            Set.Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await Context.SaveChangesAsync(ct);
        }
    }
}
