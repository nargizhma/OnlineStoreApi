namespace OnlineStoreApi.Business.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal Discount { get; set; }
        public string? Status { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CreateProductDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal Discount { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal Discount { get; set; }
        public int CategoryId { get; set; }
    }
}
