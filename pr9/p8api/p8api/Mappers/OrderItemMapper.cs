using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItemDTO ToDto(OrderItem entity)
        {
            return new OrderItemDTO
            {
                IdOrderItem = entity.IdOrderItem,
                OrderId = entity.OrderId,
                ProductId = entity.ProductId,
                Quantity = entity.Quantity,
                PriceAtOrder = entity.PriceAtOrder,
                Subtotal = entity.Subtotal
            };
        }

        public static OrderItem ToEntity(CreateOrderItemDTO dto)
        {
            return new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                PriceAtOrder = dto.PriceAtOrder
            };
        }

        public static void ApplyUpdate(OrderItem entity, UpdateOrderItemDTO dto)
        {
            entity.OrderId = dto.OrderId;
            entity.ProductId = dto.ProductId;
            entity.Quantity = dto.Quantity;
            entity.PriceAtOrder = dto.PriceAtOrder;
        }

        public static void ApplyPatch(OrderItem entity, PatchOrderItemDTO dto)
        {
            if (dto.OrderId.HasValue)
            {
                entity.OrderId = dto.OrderId.Value;
            }

            if (dto.ProductId.HasValue)
            {
                entity.ProductId = dto.ProductId.Value;
            }

            if (dto.Quantity.HasValue)
            {
                entity.Quantity = dto.Quantity.Value;
            }

            if (dto.PriceAtOrder.HasValue)
            {
                entity.PriceAtOrder = dto.PriceAtOrder.Value;
            }
        }
    }
}
