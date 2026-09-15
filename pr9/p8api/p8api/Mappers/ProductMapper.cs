using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class ProductMapper
    {
        public static ProductDTO ToDto(Product entity)
        {
            return new ProductDTO
            {
                IdProduct = entity.IdProduct,
                CategoryId = entity.CategoryId,
                ProductName = entity.ProductName,
                Description = entity.Description,
                Price = entity.Price,
                StockQuantity = entity.StockQuantity,
                IsAvailable = entity.IsAvailable,
                ImageUrl = entity.ImageUrl,
                CreatedDate = entity.CreatedDate
            };
        }

        public static Product ToEntity(CreateProductDTO dto)
        {
            return new Product
            {
                CategoryId = dto.CategoryId,
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                IsAvailable = dto.IsAvailable,
                ImageUrl = dto.ImageUrl,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static void ApplyUpdate(Product entity, UpdateProductDTO dto)
        {
            entity.CategoryId = dto.CategoryId;
            entity.ProductName = dto.ProductName;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.StockQuantity = dto.StockQuantity;
            entity.IsAvailable = dto.IsAvailable;
            entity.ImageUrl = dto.ImageUrl;
        }

        public static void ApplyPatch(Product entity, PatchProductDTO dto)
        {
            if (dto.CategoryId.HasValue)
            {
                entity.CategoryId = dto.CategoryId.Value;
            }

            if (dto.ProductName is not null)
            {
                entity.ProductName = dto.ProductName;
            }

            if (dto.Description is not null)
            {
                entity.Description = dto.Description;
            }

            if (dto.Price.HasValue)
            {
                entity.Price = dto.Price.Value;
            }

            if (dto.StockQuantity.HasValue)
            {
                entity.StockQuantity = dto.StockQuantity.Value;
            }

            if (dto.IsAvailable.HasValue)
            {
                entity.IsAvailable = dto.IsAvailable.Value;
            }

            if (dto.ImageUrl is not null)
            {
                entity.ImageUrl = dto.ImageUrl;
            }
        }
    }
}
