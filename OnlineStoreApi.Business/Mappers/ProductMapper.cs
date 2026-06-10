using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Business.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Discount = product.Discount,
                Status = product.Status,
                CategoryId = product.CategoryId,
                CategoryName = product.ProductCategory?.Name
            };
        }

        public static Product ToEntity(this CreateProductDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock,
                Discount = dto.Discount,
                CategoryId = dto.CategoryId,
                Status = "active"
            };
        }

        public static void UpdateEntity(this UpdateProductDto dto, Product product)
        {
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Discount = dto.Discount;
            product.CategoryId = dto.CategoryId;
        }
    }
}
