using OnlineStoreApi.Business.DTOs;

namespace OnlineStoreApi.Business.Services
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategoryDto>> GetAllCategoriesAsync();
        Task<ProductCategoryDto> GetCategoryByIdAsync(int id);
        Task<ProductCategoryDto> CreateCategoryAsync(CreateProductCategoryDto dto);
        Task UpdateCategoryAsync(int id, UpdateProductCategoryDto dto);
        Task DeleteCategoryAsync(int id);
    }
}
