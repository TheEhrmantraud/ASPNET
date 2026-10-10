using AutoMapper;
using p8api.DTOs;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUserRepository _users;
        private readonly IOrderStatusRepository _orderStatuses;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IUserRepository users, IOrderStatusRepository orderStatuses, IMapper mapper)
        {
            _repository = repository;
            _users = users;
            _orderStatuses = orderStatuses;
            _mapper = mapper;
        }

        public async Task<PagedResult<OrderDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _repository.GetPageAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return new PagedResult<OrderDTO>(_mapper.Map<List<OrderDTO>>(items), total);
        }

        public async Task<OrderDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<OrderDTO>(entity);
        }

        public async Task<OrderDTO> CreateAsync(CreateOrderDTO dto, CancellationToken ct)
        {
            await _users.EnsureExistsAsync(dto.UserId, $"Пользователь с Id {dto.UserId} не найден.", ct);
            await _orderStatuses.EnsureExistsAsync(dto.OrderStatusId, $"Статус заказа с Id {dto.OrderStatusId} не найден.", ct);

            var entity = _mapper.Map<Order>(dto);
            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(entity.IdOrder, ct);
        }

        public async Task<OrderDTO?> UpdateAsync(int id, UpdateOrderDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            await _users.EnsureExistsAsync(dto.UserId, $"Пользователь с Id {dto.UserId} не найден.", ct);
            await _orderStatuses.EnsureExistsAsync(dto.OrderStatusId, $"Статус заказа с Id {dto.OrderStatusId} не найден.", ct);

            _mapper.Map(dto, entity);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<OrderDTO?> PatchAsync(int id, PatchOrderDTO dto, CancellationToken ct)
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

            if (dto.OrderStatusId.HasValue)
            {
                await _orderStatuses.EnsureExistsAsync(dto.OrderStatusId.Value, $"Статус заказа с Id {dto.OrderStatusId.Value} не найден.", ct);
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

        private async Task<OrderDTO> LoadAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException($"Заказ с Id {id} не найден.");
            return _mapper.Map<OrderDTO>(entity);
        }
    }
}
