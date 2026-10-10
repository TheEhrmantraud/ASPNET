using p8api.DTOs;

namespace p8api.Services
{
    public interface IProductService : ICrudService<ProductDTO, CreateProductDTO, UpdateProductDTO, PatchProductDTO>
    {
    }
}
