using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToDto(User entity)
        {
            return new UserDTO
            {
                IdUser = entity.IdUser,
                RoleId = entity.RoleId,
                Login = entity.Login,
                PasswordHash = entity.PasswordHash,
                Surname = entity.Surname,
                NameUser = entity.NameUser,
                MiddleName = entity.MiddleName,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                IsActive = entity.IsActive,
                RegistrationDate = entity.RegistrationDate
            };
        }

        public static User ToEntity(CreateUserDTO dto)
        {
            return new User
            {
                RoleId = dto.RoleId,
                Login = dto.Login,
                PasswordHash = dto.PasswordHash,
                Surname = dto.Surname,
                NameUser = dto.NameUser,
                MiddleName = dto.MiddleName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                IsActive = dto.IsActive,
                RegistrationDate = DateTime.UtcNow
            };
        }

        public static void ApplyUpdate(User entity, UpdateUserDTO dto)
        {
            entity.RoleId = dto.RoleId;
            entity.Login = dto.Login;
            entity.PasswordHash = dto.PasswordHash;
            entity.Surname = dto.Surname;
            entity.NameUser = dto.NameUser;
            entity.MiddleName = dto.MiddleName;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Email = dto.Email;
            entity.IsActive = dto.IsActive;
        }

        public static void ApplyPatch(User entity, PatchUserDTO dto)
        {
            if (dto.RoleId.HasValue)
            {
                entity.RoleId = dto.RoleId.Value;
            }

            if (dto.Login is not null)
            {
                entity.Login = dto.Login;
            }

            if (dto.PasswordHash is not null)
            {
                entity.PasswordHash = dto.PasswordHash;
            }

            if (dto.Surname is not null)
            {
                entity.Surname = dto.Surname;
            }

            if (dto.NameUser is not null)
            {
                entity.NameUser = dto.NameUser;
            }

            if (dto.MiddleName is not null)
            {
                entity.MiddleName = dto.MiddleName;
            }

            if (dto.PhoneNumber is not null)
            {
                entity.PhoneNumber = dto.PhoneNumber;
            }

            if (dto.Email is not null)
            {
                entity.Email = dto.Email;
            }

            if (dto.IsActive.HasValue)
            {
                entity.IsActive = dto.IsActive.Value;
            }
        }
    }
}
