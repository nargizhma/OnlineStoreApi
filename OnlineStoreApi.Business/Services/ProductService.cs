using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Business.Mappers;
using OnlineStoreApi.Data.Models;
using OnlineStoreApi.Data.Repositories;

namespace OnlineStoreApi.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => p.ToDto());
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");
            return product.ToDto();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _repository.GetByCategoryAsync(categoryId);
            return products.Select(p => p.ToDto());
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            ValidateProductInput(dto);

            var product = dto.ToEntity();
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
            return product.ToDto();
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            ValidateProductInput(dto);

            dto.UpdateEntity(product);
            _repository.Update(product);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            _repository.Delete(product);
            await _repository.SaveChangesAsync();
        }

        private void ValidateProductInput(dynamic dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Product name is required");

            if (dto.Price <= 0)
                throw new ArgumentException("Product price must be greater than 0");

            if (dto.Discount < 0 || dto.Discount > 50)
                throw new ArgumentException("Discount must be between 0 and 50");

            if (dto.Stock < 0)
                throw new ArgumentException("Stock cannot be negative");
        }
    }
}
