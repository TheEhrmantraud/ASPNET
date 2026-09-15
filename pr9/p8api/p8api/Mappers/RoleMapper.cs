using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class RoleMapper
    {
        public static RoleDTO ToDto(Role entity)
        {
            return new RoleDTO
            {
                IdRole = entity.IdRole,
                RoleName = entity.RoleName,
                Description = entity.Description
            };
        }

        public static Role ToEntity(CreateRoleDTO dto)
        {
            return new Role
            {
                RoleName = dto.RoleName,
                Description = dto.Description
            };
        }

        public static void ApplyUpdate(Role entity, UpdateRoleDTO dto)
        {
            entity.RoleName = dto.RoleName;
            entity.Description = dto.Description;
        }

        public static void ApplyPatch(Role entity, PatchRoleDTO dto)
        {
            if (dto.RoleName is not null)
            {
                entity.RoleName = dto.RoleName;
            }

            if (dto.Description is not null)
            {
                entity.Description = dto.Description;
            }
        }
    }
}
