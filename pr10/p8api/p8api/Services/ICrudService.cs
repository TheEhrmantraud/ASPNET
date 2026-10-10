using p8api.DTOs;

namespace p8api.Services
{
    public interface ICrudService<TReadDto, TCreateDto, TUpdateDto, TPatchDto>
    {
        Task<PagedResult<TReadDto>> GetPageAsync(int page, int pageSize, CancellationToken ct);
        Task<TReadDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<TReadDto> CreateAsync(TCreateDto dto, CancellationToken ct);
        Task<TReadDto?> UpdateAsync(int id, TUpdateDto dto, CancellationToken ct);
        Task<TReadDto?> PatchAsync(int id, TPatchDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
