using p8api.DTOs;
using p8api.Models;

namespace p8api.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDTO ToDto(Category entity)
        {
            return new CategoryDTO
            {
                IdCategory = entity.IdCategory,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentCategoryId = entity.ParentCategoryId
            };
        }

        public static Category ToEntity(CreateCategoryDTO dto)
        {
            return new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                ParentCategoryId = dto.ParentCategoryId
            };
        }

        public static void ApplyUpdate(Category entity, UpdateCategoryDTO dto)
        {
            entity.CategoryName = dto.CategoryName;
            entity.Description = dto.Description;
            entity.ParentCategoryId = dto.ParentCategoryId;
        }

        public static void ApplyPatch(Category entity, PatchCategoryDTO dto)
        {
            if (dto.CategoryName is not null)
            {
                entity.CategoryName = dto.CategoryName;
            }

            if (dto.Description is not null)
            {
                entity.Description = dto.Description;
            }

            if (dto.ParentCategoryId.HasValue)
            {
                entity.ParentCategoryId = dto.ParentCategoryId;
            }
        }
    }
}
