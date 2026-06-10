namespace OnlineStoreApi.Business.DTOs
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CreateProductCategoryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateProductCategoryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
