using AutoMapper;
using p8api.DTOs;
using p8api.Models;
using p8api.Repositories;

namespace p8api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roles;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IRoleRepository roles, IMapper mapper)
        {
            _repository = repository;
            _roles = roles;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            (page, pageSize) = Paging.Normalize(page, pageSize);
            var items = await _repository.GetPageAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return new PagedResult<UserDTO>(_mapper.Map<List<UserDTO>>(items), total);
        }

        public async Task<UserDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<UserDTO>(entity);
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO dto, CancellationToken ct)
        {
            await _roles.EnsureExistsAsync(dto.RoleId, $"Роль с Id {dto.RoleId} не найдена.", ct);

            var entity = _mapper.Map<User>(dto);
            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(entity.IdUser, ct);
        }

        public async Task<UserDTO?> UpdateAsync(int id, UpdateUserDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            await _roles.EnsureExistsAsync(dto.RoleId, $"Роль с Id {dto.RoleId} не найдена.", ct);

            _mapper.Map(dto, entity);
            await _repository.SaveChangesAsync(ct);
            return await LoadAsync(id, ct);
        }

        public async Task<UserDTO?> PatchAsync(int id, PatchUserDTO dto, CancellationToken ct)
        {
            var entity = await _repository.FindAsync(id, ct);
            if (entity is null)
            {
                return null;
            }

            if (dto.RoleId.HasValue)
            {
                await _roles.EnsureExistsAsync(dto.RoleId.Value, $"Роль с Id {dto.RoleId.Value} не найдена.", ct);
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

        private async Task<UserDTO> LoadAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException($"Пользователь с Id {id} не найден.");
            return _mapper.Map<UserDTO>(entity);
        }
    }
}
