using p8api.DTOs;

namespace p8api.Services
{
    public interface IRoleService : ICrudService<RoleDTO, CreateRoleDTO, UpdateRoleDTO, PatchRoleDTO>
    {
    }
}
