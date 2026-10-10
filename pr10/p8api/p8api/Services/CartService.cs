using AutoMapper;
using p8api.DTOs;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IUserRepository _users;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IUserRepository users, IMapper mapper)
        {
            _repository = repository;
            _users = users;
            _mapper = mapper;
        }

        public async Task<PagedResult<CartDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _repository.GetPageAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return new PagedResult<CartDTO>(_mapper.Map<List<CartDTO>>(items), total);
        }

        public async Task<CartDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<CartDTO>(entity);
        }

        public async Task<CartDTO> CreateAsync(CreateCartDTO dto, CancellationToken ct)
        {
            await _users.EnsureExistsAsync(dto.UserId, $"Пользователь с Id {dto.UserId} не найден.", ct);

            var entity = _mapper.Map<Cart>(dto);
            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(entity.IdCart, ct);
        }

        public async Task<CartDTO?> UpdateAsync(int id, UpdateCartDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            await _users.EnsureExistsAsync(dto.UserId, $"Пользователь с Id {dto.UserId} не найден.", ct);

            _mapper.Map(dto, entity);
            entity.UpdatedDate = DateTime.Now;
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<CartDTO?> PatchAsync(int id, PatchCartDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            if (dto.UserId.HasValue)
            {
                await _users.EnsureExistsAsync(dto.UserId.Value, $"Пользователь с Id {dto.UserId.Value} не найден.", ct);
            }

            _mapper.Map(dto, entity);
            entity.UpdatedDate = DateTime.Now;
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return false;
            }

            _repository.Remove(entity);
            await _repository.SaveChangesAsync(ct);
            return true;
        }

        private async Task<CartDTO> LoadAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException($"Корзина с Id {id} не найдена.");
            return _mapper.Map<CartDTO>(entity);
        }
    }
}
