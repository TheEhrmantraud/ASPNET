using p8api.Exceptions;
using p8api.Repositories;

namespace p8api.Services
{
    public static class RepositoryExtensions
    {
        public static async Task EnsureExistsAsync<TEntity>(this IRepository<TEntity> repository, int id, string errorMessage, CancellationToken ct)
            where TEntity : class
        {
            if (!await repository.ExistsAsync(id, ct))
            {
                throw new RelatedEntityNotFoundException(errorMessage);
            }
        }
    }
}
