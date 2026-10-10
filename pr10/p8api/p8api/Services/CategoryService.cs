using AutoMapper;
using p8api.DTOs;
using p8api.Exceptions;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CategoryDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _repository.GetPageAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return new PagedResult<CategoryDTO>(_mapper.Map<List<CategoryDTO>>(items), total);
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<CategoryDTO>(entity);
        }

        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto, CancellationToken ct)
        {
            if (dto.ParentCategoryId.HasValue)
            {
                await _repository.EnsureExistsAsync(dto.ParentCategoryId.Value, $"Родительская категория с Id {dto.ParentCategoryId.Value} не найдена.", ct);
            }

            var entity = _mapper.Map<Category>(dto);
            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(entity.IdCategory, ct);
        }

        public async Task<CategoryDTO?> UpdateAsync(int id, UpdateCategoryDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            if (dto.ParentCategoryId == id)
            {
                throw new BusinessRuleException("Категория не может быть родительской для самой себя.");
            }

            if (dto.ParentCategoryId.HasValue)
            {
                await _repository.EnsureExistsAsync(dto.ParentCategoryId.Value, $"Родительская категория с Id {dto.ParentCategoryId.Value} не найдена.", ct);
            }

            _mapper.Map(dto, entity);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<CategoryDTO?> PatchAsync(int id, PatchCategoryDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            if (dto.ParentCategoryId == id)
            {
                throw new BusinessRuleException("Категория не может быть родительской для самой себя.");
            }

            if (dto.ParentCategoryId.HasValue)
            {
                await _repository.EnsureExistsAsync(dto.ParentCategoryId.Value, $"Родительская категория с Id {dto.ParentCategoryId.Value} не найдена.", ct);
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

        private async Task<CategoryDTO> LoadAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException($"Категория с Id {id} не найдена.");
            return _mapper.Map<CategoryDTO>(entity);
        }
    }
}
