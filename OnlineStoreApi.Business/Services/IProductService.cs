using OnlineStoreApi.Business.DTOs;

namespace OnlineStoreApi.Business.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task UpdateProductAsync(int id, UpdateProductDto dto);
        Task DeleteProductAsync(int id);
    }
}
