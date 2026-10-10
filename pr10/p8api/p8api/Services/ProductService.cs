using AutoMapper;
using p8api.DTOs;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categories;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, ICategoryRepository categories, IMapper mapper)
        {
            _repository = repository;
            _categories = categories;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _repository.GetPageAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return new PagedResult<ProductDTO>(_mapper.Map<List<ProductDTO>>(items), total);
        }

        public async Task<ProductDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<ProductDTO>(entity);
        }

        public async Task<ProductDTO> CreateAsync(CreateProductDTO dto, CancellationToken ct)
        {
            await _categories.EnsureExistsAsync(dto.CategoryId, $"Категория с Id {dto.CategoryId} не найдена.", ct);

            var entity = _mapper.Map<Product>(dto);
            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(entity.IdProduct, ct);
        }

        public async Task<ProductDTO?> UpdateAsync(int id, UpdateProductDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            await _categories.EnsureExistsAsync(dto.CategoryId, $"Категория с Id {dto.CategoryId} не найдена.", ct);

            _mapper.Map(dto, entity);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<ProductDTO?> PatchAsync(int id, PatchProductDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            if (dto.CategoryId.HasValue)
            {
                await _categories.EnsureExistsAsync(dto.CategoryId.Value, $"Категория с Id {dto.CategoryId.Value} не найдена.", ct);
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

        private async Task<ProductDTO> LoadAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException($"Товар с Id {id} не найден.");
            return _mapper.Map<ProductDTO>(entity);
        }
    }
}
