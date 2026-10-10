using AutoMapper;
using p8api.DTOs;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepository _repository;
        private readonly IMapper _mapper;

        public OrderStatusService(IOrderStatusRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<OrderStatusDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _repository.GetPageAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return new PagedResult<OrderStatusDTO>(_mapper.Map<List<OrderStatusDTO>>(items), total);
        }

        public async Task<OrderStatusDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<OrderStatusDTO>(entity);
        }

        public async Task<OrderStatusDTO> CreateAsync(CreateOrderStatusDTO dto, CancellationToken ct)
        {
            var entity = _mapper.Map<OrderStatus>(dto);
            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(entity.IdOrderStatus, ct);
        }

        public async Task<OrderStatusDTO?> UpdateAsync(int id, UpdateOrderStatusDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            _mapper.Map(dto, entity);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<OrderStatusDTO?> PatchAsync(int id, PatchOrderStatusDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
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

        private async Task<OrderStatusDTO> LoadAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException($"Статус заказа с Id {id} не найден.");
            return _mapper.Map<OrderStatusDTO>(entity);
        }
    }
}
