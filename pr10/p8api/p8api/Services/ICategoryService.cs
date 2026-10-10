using p8api.DTOs;

namespace p8api.Services
{
    public interface ICategoryService : ICrudService<CategoryDTO, CreateCategoryDTO, UpdateCategoryDTO, PatchCategoryDTO>
    {
    }
}
