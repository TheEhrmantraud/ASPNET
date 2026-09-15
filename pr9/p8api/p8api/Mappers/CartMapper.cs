using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class CartMapper
    {
        public static CartDTO ToDto(Cart entity)
        {
            return new CartDTO
            {
                IdCart = entity.IdCart,
                UserId = entity.UserId,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public static Cart ToEntity(CreateCartDTO dto)
        {
            return new Cart
            {
                UserId = dto.UserId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
        }

        public static void ApplyUpdate(Cart entity, UpdateCartDTO dto)
        {
            entity.UserId = dto.UserId;
            entity.UpdatedDate = DateTime.UtcNow;
        }

        public static void ApplyPatch(Cart entity, PatchCartDTO dto)
        {
            if (dto.UserId.HasValue)
            {
                entity.UserId = dto.UserId.Value;
            }

            entity.UpdatedDate = DateTime.UtcNow;
        }
    }
}
