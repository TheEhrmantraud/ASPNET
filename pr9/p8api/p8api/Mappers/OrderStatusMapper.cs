using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class OrderStatusMapper
    {
        public static OrderStatusDTO ToDto(OrderStatus entity)
        {
            return new OrderStatusDTO
            {
                IdOrderStatus = entity.IdOrderStatus,
                StatusName = entity.StatusName
            };
        }

        public static OrderStatus ToEntity(CreateOrderStatusDTO dto)
        {
            return new OrderStatus
            {
                StatusName = dto.StatusName
            };
        }

        public static void ApplyUpdate(OrderStatus entity, UpdateOrderStatusDTO dto)
        {
            entity.StatusName = dto.StatusName;
        }

        public static void ApplyPatch(OrderStatus entity, PatchOrderStatusDTO dto)
        {
            if (dto.StatusName is not null)
            {
                entity.StatusName = dto.StatusName;
            }
        }
    }
}
