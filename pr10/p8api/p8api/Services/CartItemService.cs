using AutoMapper;
using p8api.DTOs;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _repository;
        private readonly ICartRepository _carts;
        private readonly IProductRepository _products;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepository repository, ICartRepository carts, IProductRepository products, IMapper mapper)
        {
            _repository = repository;
            _carts = carts;
            _products = products;
            _mapper = mapper;
        }

        public async Task<PagedResult<CartItemDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _repository.GetPageAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return new PagedResult<CartItemDTO>(_mapper.Map<List<CartItemDTO>>(items), total);
        }

        public async Task<CartItemDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<CartItemDTO>(entity);
        }

        public async Task<CartItemDTO> CreateAsync(CreateCartItemDTO dto, CancellationToken ct)
        {
            await _carts.EnsureExistsAsync(dto.CartId, $"Корзина с Id {dto.CartId} не найдена.", ct);
            await _products.EnsureExistsAsync(dto.ProductId, $"Товар с Id {dto.ProductId} не найден.", ct);

            var entity = _mapper.Map<CartItem>(dto);
            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(entity.IdCartItem, ct);
        }

        public async Task<CartItemDTO?> UpdateAsync(int id, UpdateCartItemDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            await _carts.EnsureExistsAsync(dto.CartId, $"Корзина с Id {dto.CartId} не найдена.", ct);
            await _products.EnsureExistsAsync(dto.ProductId, $"Товар с Id {dto.ProductId} не найден.", ct);

            _mapper.Map(dto, entity);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<CartItemDTO?> PatchAsync(int id, PatchCartItemDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            if (dto.CartId.HasValue)
            {
                await _carts.EnsureExistsAsync(dto.CartId.Value, $"Корзина с Id {dto.CartId.Value} не найдена.", ct);
            }

            if (dto.ProductId.HasValue)
            {
                await _products.EnsureExistsAsync(dto.ProductId.Value, $"Товар с Id {dto.ProductId.Value} не найден.", ct);
            }

            _mapper.Map(dto, entity);
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

        private async Task<CartItemDTO> LoadAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException($"Позиция корзины с Id {id} не найдена.");
            return _mapper.Map<CartItemDTO>(entity);
        }
    }
}
