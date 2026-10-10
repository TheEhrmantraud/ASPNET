namespace p8api.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetPageAsync(int page, int pageSize, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct);
        Task<TEntity?> FindAsync(int id, CancellationToken ct);
        Task<bool> ExistsAsync(int id, CancellationToken ct);
        Task AddAsync(TEntity entity, CancellationToken ct);
        void Remove(TEntity entity);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
