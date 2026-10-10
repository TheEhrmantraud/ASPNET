using p8api.DTOs;

namespace p8api.Services
{
    public interface IUserService : ICrudService<UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>
    {
    }
}
