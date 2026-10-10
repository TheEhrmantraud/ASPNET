using AutoMapper;
using p8api.DTOs;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repository;
        private readonly IOrderRepository _orders;
        private readonly IProductRepository _products;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository repository, IOrderRepository orders, IProductRepository products, IMapper mapper)
        {
            _repository = repository;
            _orders = orders;
            _products = products;
            _mapper = mapper;
        }

        public async Task<PagedResult<OrderItemDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _repository.GetPageAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return new PagedResult<OrderItemDTO>(_mapper.Map<List<OrderItemDTO>>(items), total);
        }

        public async Task<OrderItemDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<OrderItemDTO>(entity);
        }

        public async Task<OrderItemDTO> CreateAsync(CreateOrderItemDTO dto, CancellationToken ct)
        {
            await _orders.EnsureExistsAsync(dto.OrderId, $"Заказ с Id {dto.OrderId} не найден.", ct);
            await _products.EnsureExistsAsync(dto.ProductId, $"Товар с Id {dto.ProductId} не найден.", ct);

            var entity = _mapper.Map<OrderItem>(dto);
            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(entity.IdOrderItem, ct);
        }

        public async Task<OrderItemDTO?> UpdateAsync(int id, UpdateOrderItemDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            await _orders.EnsureExistsAsync(dto.OrderId, $"Заказ с Id {dto.OrderId} не найден.", ct);
            await _products.EnsureExistsAsync(dto.ProductId, $"Товар с Id {dto.ProductId} не найден.", ct);

            _mapper.Map(dto, entity);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<OrderItemDTO?> PatchAsync(int id, PatchOrderItemDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            if (dto.OrderId.HasValue)
            {
                await _orders.EnsureExistsAsync(dto.OrderId.Value, $"Заказ с Id {dto.OrderId.Value} не найден.", ct);
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

        private async Task<OrderItemDTO> LoadAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException($"Позиция заказа с Id {id} не найдена.");
            return _mapper.Map<OrderItemDTO>(entity);
        }
    }
}
