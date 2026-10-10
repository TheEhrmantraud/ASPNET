using p8api.DTOs;

namespace p8api.Services
{
    public interface IOrderService : ICrudService<OrderDTO, CreateOrderDTO, UpdateOrderDTO, PatchOrderDTO>
    {
    }
}
