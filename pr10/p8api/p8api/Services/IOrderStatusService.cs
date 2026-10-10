using p8api.DTOs;

namespace p8api.Services
{
    public interface IOrderStatusService : ICrudService<OrderStatusDTO, CreateOrderStatusDTO, UpdateOrderStatusDTO, PatchOrderStatusDTO>
    {
    }
}
