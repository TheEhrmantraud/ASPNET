using p8api.DTOs;

namespace p8api.Services
{
    public interface IOrderItemService : ICrudService<OrderItemDTO, CreateOrderItemDTO, UpdateOrderItemDTO, PatchOrderItemDTO>
    {
    }
}
