using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class OrderMapper
    {
        public static OrderDTO ToDto(Order entity)
        {
            return new OrderDTO
            {
                IdOrder = entity.IdOrder,
                UserId = entity.UserId,
                OrderStatusId = entity.OrderStatusId,
                TotalAmount = entity.TotalAmount,
                DeliveryAddress = entity.DeliveryAddress,
                PaymentMethod = entity.PaymentMethod,
                Notes = entity.Notes,
                OrderDate = entity.OrderDate
            };
        }

        public static Order ToEntity(CreateOrderDTO dto)
        {
            return new Order
            {
                UserId = dto.UserId,
                OrderStatusId = dto.OrderStatusId,
                TotalAmount = dto.TotalAmount,
                DeliveryAddress = dto.DeliveryAddress,
                PaymentMethod = dto.PaymentMethod,
                Notes = dto.Notes,
                OrderDate = DateTime.UtcNow
            };
        }

        public static void ApplyUpdate(Order entity, UpdateOrderDTO dto)
        {
            entity.UserId = dto.UserId;
            entity.OrderStatusId = dto.OrderStatusId;
            entity.TotalAmount = dto.TotalAmount;
            entity.DeliveryAddress = dto.DeliveryAddress;
            entity.PaymentMethod = dto.PaymentMethod;
            entity.Notes = dto.Notes;
        }

        public static void ApplyPatch(Order entity, PatchOrderDTO dto)
        {
            if (dto.UserId.HasValue)
            {
                entity.UserId = dto.UserId.Value;
            }

            if (dto.OrderStatusId.HasValue)
            {
                entity.OrderStatusId = dto.OrderStatusId.Value;
            }

            if (dto.TotalAmount.HasValue)
            {
                entity.TotalAmount = dto.TotalAmount.Value;
            }

            if (dto.DeliveryAddress is not null)
            {
                entity.DeliveryAddress = dto.DeliveryAddress;
            }

            if (dto.PaymentMethod is not null)
            {
                entity.PaymentMethod = dto.PaymentMethod;
            }

            if (dto.Notes is not null)
            {
                entity.Notes = dto.Notes;
            }
        }
    }
}
