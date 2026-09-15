using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class CartItemMapper
    {
        public static CartItemDTO ToDto(CartItem entity)
        {
            return new CartItemDTO
            {
                IdCartItem = entity.IdCartItem,
                CartId = entity.CartId,
                ProductId = entity.ProductId,
                Quantity = entity.Quantity,
                AddedDate = entity.AddedDate
            };
        }

        public static CartItem ToEntity(CreateCartItemDTO dto)
        {
            return new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                AddedDate = DateTime.UtcNow
            };
        }

        public static void ApplyUpdate(CartItem entity, UpdateCartItemDTO dto)
        {
            entity.CartId = dto.CartId;
            entity.ProductId = dto.ProductId;
            entity.Quantity = dto.Quantity;
        }

        public static void ApplyPatch(CartItem entity, PatchCartItemDTO dto)
        {
            if (dto.CartId.HasValue)
            {
                entity.CartId = dto.CartId.Value;
            }

            if (dto.ProductId.HasValue)
            {
                entity.ProductId = dto.ProductId.Value;
            }

            if (dto.Quantity.HasValue)
            {
                entity.Quantity = dto.Quantity.Value;
            }
        }
    }
}
