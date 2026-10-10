using p8api.DTOs;

namespace p8api.Services
{
    public interface ICartService : ICrudService<CartDTO, CreateCartDTO, UpdateCartDTO, PatchCartDTO>
    {
    }
}
