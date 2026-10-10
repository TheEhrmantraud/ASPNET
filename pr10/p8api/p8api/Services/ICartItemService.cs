using p8api.DTOs;

namespace p8api.Services
{
    public interface ICartItemService : ICrudService<CartItemDTO, CreateCartItemDTO, UpdateCartItemDTO, PatchCartItemDTO>
    {
    }
}
