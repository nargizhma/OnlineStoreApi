using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Business.Mappers
{
    public static class ProductCategoryMapper
    {
        public static ProductCategoryDto ToDto(this ProductCategory category)
        {
            return new ProductCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public static ProductCategory ToEntity(this CreateProductCategoryDto dto)
        {
            return new ProductCategory
            {
                Name = dto.Name,
                Description = dto.Description
            };
        }

        public static void UpdateEntity(this UpdateProductCategoryDto dto, ProductCategory category)
        {
            category.Name = dto.Name;
            category.Description = dto.Description;
        }
    }
}
