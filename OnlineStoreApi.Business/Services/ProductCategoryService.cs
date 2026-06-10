using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Business.Mappers;
using OnlineStoreApi.Data.Models;
using OnlineStoreApi.Data.Repositories;

namespace OnlineStoreApi.Business.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _repository;

        public ProductCategoryService(IProductCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(c => c.ToDto());
        }

        public async Task<ProductCategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");
            return category.ToDto();
        }

        public async Task<ProductCategoryDto> CreateCategoryAsync(CreateProductCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Category name is required");

            var category = dto.ToEntity();
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();
            return category.ToDto();
        }

        public async Task UpdateCategoryAsync(int id, UpdateProductCategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Category name is required");

            dto.UpdateEntity(category);
            _repository.Update(category);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");

            if (category.Products.Any())
                throw new InvalidOperationException("Cannot delete category with products");

            _repository.Delete(category);
            await _repository.SaveChangesAsync();
        }
    }
}
